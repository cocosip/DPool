using DPool.Scheduling;
using DPool.Utility;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DPool.GenericsPool
{
    /// <summary>泛型数据池
    /// </summary>
    public class GenericsDataPool<T> : IGenericsDataPool<T> where T : class, new()
    {
        /// <summary>是否正在运行
        /// </summary>
        public bool IsRunning { get; }

        /// <summary>身份标志
        /// </summary>
        public GenericsDataPoolIdentifier Identifier { get; }

        private readonly ILogger _logger;
        private readonly DataPoolOption _option;
        private readonly GenericsDataPoolOption<T> _genericsOption;
        private readonly IScheduleService _scheduleService;
        private readonly IDPoolKeyGenerator _dPoolKeyGenerator;
        private readonly IRedisClientProxy _redisClientProxy;

        private int _isRunning = 0;
        private int _isLoading = 0;
        private Func<T, string> _idSelector;
        private readonly ConcurrentDictionary<string, DataFuture<T>> _processDict;

        public GenericsDataPool(ILogger<GenericsDataPool<T>> logger, IOptions<DataPoolOption> option, GenericsDataPoolOption<T> genericsOption, IScheduleService scheduleService, IDPoolKeyGenerator dPoolKeyGenerator, IRedisClientProxy redisClientProxy)
        {
            _logger = logger;
            _option = option.Value;
            _genericsOption = genericsOption;
            _scheduleService = scheduleService;
            _dPoolKeyGenerator = dPoolKeyGenerator;
            _redisClientProxy = redisClientProxy;

            Identifier = BuildIdentifier(_genericsOption);

            _idSelector = (Func<T, string>)_genericsOption.IdSelector;
            _processDict = new ConcurrentDictionary<string, DataFuture<T>>();
        }


        /// <summary>添加数据
        /// </summary>
        /// <param name="value"></param>
        public long Write(T[] value)
        {
            var key = _dPoolKeyGenerator.GenerateDataKey(_genericsOption.Group, _genericsOption.DataType);
            var client = _redisClientProxy.GetClient();
            return client.RPush<T>(key, value);
        }

        /// <summary>获取指定数量的数据
        /// </summary>
        /// <param name="count"></param>
        public T[] Get(int count)
        {
            var key = _dPoolKeyGenerator.GenerateDataKey(_genericsOption.Group, _genericsOption.DataType);
            var lockName = _dPoolKeyGenerator.GenerateDataLockName(_genericsOption.Group, _genericsOption.DataType);
            var client = _redisClientProxy.GetClient();
            var dataLock = client.Lock(lockName, 2);

            var processIndexKey = _dPoolKeyGenerator.GenerateProcessDataIndexKey(_genericsOption.Group, _genericsOption.ProcessGroup, _genericsOption.DataType);
            var dataFeatures = new List<DataFuture<T>>();

            try
            {
                var value = client.LRange<T>(key, 0, count).Where(x => x != null).ToArray();
                if (value.Length <= 0)
                {
                    return null;
                }
                //当前数据的唯一键
                var ids = value.Select(x => _idSelector(x)).Distinct().ToArray();
                if (ids.Length <= 0)
                {
                    _logger.LogDebug("获取类型'{0}'的数据,去重后当前唯一id值的集合为空。", typeof(T).FullName);
                }

                var time = DateTimeUtil.ToInt32(DateTime.Now);

                client.StartPipe(p =>
                {
                    //索引
                    p.RPush(processIndexKey, ids);

                    foreach (var item in value)
                    {
                        //当前id
                        var id = _idSelector(item);

                        //当前数据Key
                        var processDataKey = _dPoolKeyGenerator.GenerateProcessDataKey(_genericsOption.Group, _genericsOption.ProcessGroup, _genericsOption.DataType, id);

                        p.HMSet(processDataKey, DPoolConsts.PROCESS_DATA_DATETIME_FIELD, time, DPoolConsts.PROCESS_DATA_DATA_FIELD, item);

                        //添加到列表
                        dataFeatures.Add(new DataFuture<T>(id, item));
                    }

                    //移除
                    p.LTrim(key, count + 1, -1);
                });

                //添加到处理中的
                foreach (var dataFeature in dataFeatures)
                {
                    if (_processDict.ContainsKey(dataFeature.Id))
                    {
                        if (!_processDict.TryAdd(dataFeature.Id, dataFeature))
                        {
                            _logger.LogWarning("添加待归还的数据失败,当前数据池:{0},数据Id:'{1}'.", Identifier, dataFeature.Id);
                        }
                    }
                }

                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取数据池数据出错,当前数据池信息:{0},异常信息:{1}", Identifier, ex.Message);
                return null;
            }
            finally
            {
                dataLock?.Unlock();
            }
        }

        /// <summary>归还数据,将数据归还到主链表
        /// </summary>
        /// <param name="value"></param>
        public void Return(params T[] value)
        {
            var key = _dPoolKeyGenerator.GenerateDataKey(_genericsOption.Group, _genericsOption.DataType);
            var processIndexKey = _dPoolKeyGenerator.GenerateProcessDataIndexKey(_genericsOption.Group, _genericsOption.ProcessGroup, _genericsOption.DataType);
            var client = _redisClientProxy.GetClient();

            try
            {
                var ids = value.Select(x => _idSelector(x)).ToArray();
                client.StartPipe(p =>
                {
                    foreach (var item in value)
                    {
                        var id = _idSelector(item);

                        //当前数据Key
                        var processDataKey = _dPoolKeyGenerator.GenerateProcessDataKey(_genericsOption.Group, _genericsOption.ProcessGroup, _genericsOption.DataType, id);

                        p.Del(processDataKey);
                        ////删除数据
                        //p.HDel(processDataKey);
                        //删除索引
                        p.LRem(processIndexKey, 1, id);
                    }

                    //添加到数据中
                    p.RPush<T>(key, value);
                });

                //从本地字典中删除
                RemoveLocalProcess(ids);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "归还数据时出错,当前数据池信息:{0},异常信息:{1}", Identifier, ex.Message);
            }
        }

        /// <summary>删除数据,从取走的数据中删除
        /// </summary>
        /// <param name="value"></param>
        public void Release(params T[] value)
        {
            var processIndexKey = _dPoolKeyGenerator.GenerateProcessDataIndexKey(_genericsOption.Group, _genericsOption.ProcessGroup, _genericsOption.DataType);
            var client = _redisClientProxy.GetClient();

            try
            {
                var ids = value.Select(x => _idSelector(x)).ToArray();
                client.StartPipe(p =>
                {
                    foreach (var item in value)
                    {
                        var id = _idSelector(item);
                        //当前数据Key
                        var processDataKey = _dPoolKeyGenerator.GenerateProcessDataKey(_genericsOption.Group, _genericsOption.ProcessGroup, _genericsOption.DataType, id);

                        p.Del(processDataKey);
                        ////删除数据
                        //p.HDel(processDataKey);
                        //删除索引
                        p.LRem(processIndexKey, 1, id);
                    }
                });


                //从本地字典中删除
                RemoveLocalProcess(ids);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "归还数据时出错,当前数据池信息:{0},异常信息:{1}", Identifier, ex.Message);
            }
        }

        /// <summary>运行
        /// </summary>
        public void Start()
        {
            if (_isRunning == 1)
            {
                return;
            }

            //加载数据到本地
            LoadLocalProcess();

            StartScanTimeoutDataTask();

            Interlocked.Exchange(ref _isRunning, 1);
            _logger.LogInformation("GenericsDataPool 运行成功! 当前信息:{0}", Identifier);
        }


        /// <summary>停止
        /// </summary>
        public void Shutdown()
        {
            if (_isRunning == 0)
            {
                return;
            }

            StopScanTimeoutDataTask();

            _processDict.Clear();

            Interlocked.Exchange(ref _isRunning, 0);
            _logger.LogInformation("GenericsDataPool 停止成功! 当前信息:{0}", Identifier);
        }



        #region Private method

        private void StartScanTimeoutDataTask()
        {
            _scheduleService.StartTask($"{_genericsOption.Group}.{GetType().FullName}.ScanTimeoutData", ScanTimeoutData, 1000, _option.ScanTimeoutDataInterval);
        }

        private void StopScanTimeoutDataTask()
        {
            _scheduleService.StopTask($"{_genericsOption.Group}.{GetType().FullName}.ScanTimeoutData");
        }

        /// <summary>扫描过期的数据
        /// </summary>
        private void ScanTimeoutData()
        {
            var timeoutList = new List<DataFuture<T>>();
            foreach (var entry in _processDict)
            {
                if (entry.Value.IsTimeout(_option.DataTimeoutSeconds))
                {
                    timeoutList.Add(entry.Value);
                }
            }
            var timeoutValue = timeoutList.Select(x => x.Data).ToArray();
            if (timeoutValue.Length > 0)
            {
                //归还
                Return(timeoutValue);
            }
            else
            {
                _logger.LogDebug("本次自动归还数据个数为:'{0}'.", timeoutValue.Length);
            }
        }

        /// <summary>加载本地数据
        /// </summary>
        private Task LoadLocalProcess()
        {
            return Task.Run(() =>
            {
                if (_isLoading == 1)
                {
                    _logger.LogWarning("正在加载Redis数据到本地,请不要重复操作!");
                    return;
                }
                //变成加载中
                Interlocked.Exchange(ref _isLoading, 1);

                try
                {
                    var processIndexKey = _dPoolKeyGenerator.GenerateProcessDataIndexKey(_genericsOption.Group, _genericsOption.ProcessGroup, _genericsOption.DataType);
                    var client = _redisClientProxy.GetClient();

                    //获取索引全部数据
                    var ids = client.LRange(processIndexKey, 0, -1);
                    if (ids.Length > 0)
                    {
                        var result = client.StartPipe(p =>
                        {
                            foreach (var id in ids)
                            {
                                //当前数据Key
                                var processDataKey = _dPoolKeyGenerator.GenerateProcessDataKey(_genericsOption.Group, _genericsOption.ProcessGroup, _genericsOption.DataType, id);

                                p.HGet<int>(processDataKey, DPoolConsts.PROCESS_DATA_DATETIME_FIELD);
                                p.HGet<T>(processDataKey, DPoolConsts.PROCESS_DATA_DATA_FIELD);
                            }
                        });

                        int datetime = DateTimeUtil.ToInt32(DateTime.Now.AddDays(-1));
                        T data = null;
                        for (int i = 0; i < result.Length; i++)
                        {
                            if (i % 2 == 0)
                            {
                                datetime = Convert.ToInt32(result[i]);
                            }
                            else
                            {
                                data = result[i] as T;
                                if (data != null)
                                {
                                    var id = _idSelector(data);
                                    var createdOn = DateTimeUtil.ToDateTime(datetime);
                                    var dataFuture = new DataFuture<T>(id, data, createdOn);
                                    //加入到本地队列中
                                    if (_processDict.TryAdd(id, dataFuture))
                                    {
                                        _logger.LogDebug("将处理中数据加入本地队列失败,数据池信息:{0},Id:'{1}',创建时间:'{2}'.", Identifier, id, createdOn.ToString("yyyy-MM-dd HH:mm:ss"));
                                    }
                                }
                                else
                                {
                                    _logger.LogDebug("从Redis读取处理中数据数据为空,数据池信息:{0}", Identifier);
                                }
                            }

                        }

                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "从Redis中加载处理中的数据出错,当前泛型数据池信息:{0},异常信息:{1}.", Identifier, ex.Message);
                    throw;
                }
                finally
                {
                    Interlocked.Exchange(ref _isLoading, 0);
                }

            });
        }



        /// <summary>生成标志
        /// </summary>
        private GenericsDataPoolIdentifier BuildIdentifier(GenericsDataPoolOption option)
        {
            var identifier = new GenericsDataPoolIdentifier()
            {
                Group = option.Group,
                DataType = option.DataType
            };
            return identifier;
        }


        /// <summary>移除本地的数据
        /// </summary>
        private void RemoveLocalProcess(params string[] ids)
        {
            foreach (var id in ids)
            {
                if (!_processDict.TryRemove(id, out DataFuture<T> _))
                {
                    _logger.LogTrace("本地集合中已经移除该数据,数据池:{0},数据Id:'{1}'.", Identifier, id);
                }
            }
        }
        #endregion

    }
}
