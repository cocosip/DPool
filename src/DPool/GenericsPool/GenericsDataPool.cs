using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace DPool.GenericsPool
{

    /// <summary>泛型数据池
    /// </summary>
    public class GenericsDataPool
    {
        /// <summary>标志
        /// </summary>
        public GenericsDataPoolIdentifier Identifier { get; protected set; }

        public void Start()
        {

        }
    }

    /// <summary>泛型数据池
    /// </summary>
    public class GenericsDataPool<T> : GenericsDataPool
    {

        private readonly ILogger _logger;
        private readonly GenericsDataPoolOption<T> _option;
        private readonly ConcurrentDictionary<string, string> _processDict;
        public GenericsDataPool(ILogger<GenericsDataPool<T>> logger, GenericsDataPoolOption<T> option)
        {
            _logger = logger;
            _option = option;

            Identifier = BuildIdentifier(_option);
            _processDict = new ConcurrentDictionary<string, string>();
        }

        /// <summary>添加数据
        /// </summary>
        /// <param name="value"></param>
        public void AddData(T[] value)
        {
            var key = "";
            var writeIndex = "";

            RedisHelper.StartPipe(p =>
            {
                p.LPush<T>(key, value);
                p.IncrBy(writeIndex, value.Length);
            });
        }

        public void GetData(int count)
        {
            var key = "";
            var lockKey = "";
            var dataLock = RedisHelper.Lock(lockKey, 2);
            try
            {
                var value = RedisHelper.LRange<T>(key, 0, count);

                //var ids = value.Select(x => _option.IdSelector(x)).ToArray();
                var ids = new string[] { "" };
                //移动到当前私有集合中
                var indexKey = "";

                RedisHelper.StartPipe(p =>
                {
                    //发布到新链表
                    var date = DateTime.Now.Second;

                    //索引
                    p.RPush(indexKey, ids);

                    foreach (var item in value)
                    {
                        var id = "";
                        //var id = _option.IdSelector(item);
                        p.HMSet(id, "", "", "", item);
                    }

                    //移除
                    p.LTrim(key, count + 1, -1);
                });



            }
            catch (Exception ex)
            {

            }
            finally
            {
                dataLock?.Unlock();
            }
        }


        public void Remove(params string[] ids)
        {
            var key = "";
            var lockKey = "";
            var dataLock = RedisHelper.Lock(lockKey, 2);
            try
            {

                //移动到当前私有集合中
                var indexKey = "";


                RedisHelper.StartPipe(p =>
                {
                    foreach (var id in ids)
                    {

                    }

                    //移除索引
                    p.LRem("", 1, ids);


                });

            }
            catch (Exception ex)
            {

            }
            finally
            {
                dataLock?.Unlock();
            }
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

    }
}
