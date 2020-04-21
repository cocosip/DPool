﻿using DPool.RedisFx;
using DPool.RedisFx.List;
using DPool.Scheduling;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DPool
{
    /// <summary>数据池
    /// </summary>
    public class DataPool
    {
        public bool IsRunning { get; private set; } = false;

        private readonly ILogger _logger;
        private readonly DataPoolOption _option;
        private readonly IDPoolKeyGenerator _dPoolKeyGenerator;

        private readonly IScheduleService _scheduleService;

        private readonly IRedisClientProxy _redisClientProxy;
        private readonly IRedisListFactory _redisListFactory;


        /// <summary>Ctor
        /// </summary>
        public DataPool(ILogger<DataPool> logger,IServiceProvider provider, IOptions<DataPoolOption> option, IDPoolKeyGenerator dPoolKeyGenerator, IRedisClientProxy redisClientProxy, IRedisListFactory redisListFactory)
        {
            _logger = logger;
            _option = option.Value;
            _dPoolKeyGenerator = dPoolKeyGenerator;

            _scheduleService = provider.GetService<IScheduleService>();

            _redisClientProxy = redisClientProxy;
            _redisListFactory = redisListFactory;
        }

        /// <summary>添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> WriteAsync<T>(string group = "", params T[] value)
        {
            group = GetGroup(group);
            var key = _dPoolKeyGenerator.GenerateDataKey(group, typeof(T));
            var client = _redisClientProxy.GetClient();
            return await client.RPushAsync<T>(key, value);
        }

        /// <summary>获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task<T[]> GetAsync<T>(int count, string group = "") where T : IRedisListData
        {
            group = GetGroup(group);

            var lockKey = _dPoolKeyGenerator.GenerateDataLockKey(group, typeof(T));
            var client = _redisClientProxy.GetClient();
            var l = client.Lock(lockKey, _option.DataLockSeconds);

            var list = _redisListFactory.Get<DataFuture<T>>(group);

            var key = _dPoolKeyGenerator.GenerateDataKey(group, typeof(T));
            try
            {
                //从链表中获取数据
                var value = await client.LRangeAsync<T>(key, 0, count);

                //将数据添加到处理中的数据 
                var dataFutures = value.Select(x => new DataFuture<T>(x)).ToArray();

                //添加到进行中的数据链表
                list.Add(dataFutures);

                //删除获取的数据
                await client.LTrimAsync(key, 0, count);

                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取DataPool数据出错了,Group:'{0}',类型:'{1}',异常信息:{2}", group, typeof(T).FullName, ex.Message);
                return default;
            }
            finally
            {
                l?.Unlock();
            }

        }

        /// <summary>归还数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task ReturnAsync<T>(string group = "", params T[] value) where T : IRedisListData
        {
            group = GetGroup(group);

            var client = _redisClientProxy.GetClient();
            var list = _redisListFactory.Get<DataFuture<T>>(group);
            var key = _dPoolKeyGenerator.GenerateDataKey(group, typeof(T));

            try
            {
                var ids = value.Select(x => ((IRedisListData)x).SelectId()).ToArray();

                //从链表删除
                list.Remove(ids);

                //将数据重新添加到列表
                await client.RPushAsync(key, value);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "归还Redis数据出错,Group:'{0}',类型:'{1}',异常信息:{1}", group, typeof(T).FullName, ex.Message);
                throw ex;
            }

        }

        /// <summary>释放数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task RleaseAsync<T>(string group = "", params T[] value) where T : IRedisListData
        {
            group = GetGroup(group);

            var client = _redisClientProxy.GetClient();
            var list = _redisListFactory.Get<DataFuture<T>>(group);

            try
            {
                var ids = value.Select(x => ((IRedisListData)x).SelectId()).ToArray();

                //从链表删除
                list.Remove(ids);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "归还Redis数据出错,Group:'{0}',类型:'{1}',异常信息:{1}", group, typeof(T).FullName, ex.Message);
                throw ex;
            }
            await Task.FromResult(0);
        }


        public void Start()
        {
            if (IsRunning)
            {
                _logger.LogInformation("DataPool正在运行,请勿重复运行!");
                return;
            }




            IsRunning = true;
        }


        public void Shutdown()
        {

        }


        private string GetGroup(string group)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                group = _option.DefaultGroup;
            }
            return group;
        }



    }
}