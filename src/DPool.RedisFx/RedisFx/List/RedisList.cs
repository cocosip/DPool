using Microsoft.Extensions.Logging;
using System.Linq;

namespace DPool.RedisFx.List
{
    /// <summary>Redis链表
    /// </summary>
    public class RedisList<T> : IRedisList<T> where T : IRedisListData
    {
        private readonly ILogger _logger;
        private readonly RedisListDescriptor _descriptor;
        private readonly IRedisClientProxy _clientProxy;
        private readonly IRedisListKeyGenerator _redisListKeyGenerator;

        public RedisList(ILogger<RedisList<T>> logger, RedisListDescriptor descriptor, IRedisClientProxy clientProxy, IRedisListKeyGenerator redisListKeyGenerator)
        {
            _logger = logger;
            _clientProxy = clientProxy;
            _descriptor = descriptor;
            _redisListKeyGenerator = redisListKeyGenerator;
        }


        /// <summary>添加数据元素
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Add(params T[] value)
        {
            var indexKey = _redisListKeyGenerator.GenerateIndexKey(_descriptor.DataType, _descriptor.Group);

            _clientProxy.GetClient().StartPipe(p =>
            {
                foreach (var item in value)
                {
                    var id = ((IRedisListData)item).SelectId();
                    var key = _redisListKeyGenerator.GenerateDataKey(_descriptor.DataType, _descriptor.Group, id);
                    //索引
                    p.RPush(indexKey, key);
                    //数据
                    p.Set(key, item);
                }

            });

        }

        /// <summary>删除数据
        /// </summary>
        public void Remove(params T[] value)
        {
            var indexKey = _redisListKeyGenerator.GenerateIndexKey(_descriptor.DataType, _descriptor.Group);

            _clientProxy.GetClient().StartPipe(p =>
            {
                foreach (var item in value)
                {
                    var id = ((IRedisListData)item).SelectId();
                    var key = _redisListKeyGenerator.GenerateDataKey(_descriptor.DataType, _descriptor.Group, id);

                    //索引删除
                    p.LRem(indexKey, 1, key);

                    //数据删除
                    p.Del(key);
                }

            });
        }


        /// <summary>根据Id集合删除数据
        /// </summary>
        public void Remove(params string[] ids)
        {
            var indexKey = _redisListKeyGenerator.GenerateIndexKey(_descriptor.DataType, _descriptor.Group);

            _clientProxy.GetClient().StartPipe(p =>
            {
                foreach (var id in ids)
                {
                    var key = _redisListKeyGenerator.GenerateDataKey(_descriptor.DataType, _descriptor.Group, id);

                    //索引删除
                    p.LRem(indexKey, 1, key);

                    //数据删除
                    p.Del(key);
                }
            });
        }

        /// <summary>获取指定个数的数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public T[] Get(int count)
        {
            var indexKey = _redisListKeyGenerator.GenerateIndexKey(_descriptor.DataType, _descriptor.Group);
            var client = _clientProxy.GetClient();

            var keys = client.LRange<string>(indexKey, 0, count).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            return client.MGet<T>(keys);
        }
    }
}
