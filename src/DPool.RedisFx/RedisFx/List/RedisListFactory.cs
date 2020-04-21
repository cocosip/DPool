using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;

namespace DPool.RedisFx.List
{
    /// <summary>Redis链表工厂
    /// </summary>
    public class RedisListFactory : IRedisListFactory
    {

        private readonly ILogger _logger;
        private readonly IServiceProvider _provider;
        private readonly RedisListOption _option;
        private readonly ConcurrentDictionary<RedisListIdentifier, IRedisList> _redisListDict;
        public RedisListFactory(ILogger<RedisListFactory> logger, IServiceProvider provider, IOptions<RedisListOption> option)
        {
            _logger = logger;
            _provider = provider;
            _option = option.Value;

            _redisListDict = new ConcurrentDictionary<RedisListIdentifier, IRedisList>();
        }


        /// <summary>获取链表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group"></param>
        /// <returns></returns>
        public IRedisList<T> Get<T>(string group = "") where T : IRedisListData
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                group = _option.DefaultGroup;
            }

            var identifier = new RedisListIdentifier()
            {
                Group = group,
                DataType = typeof(T)
            };

            if (!_redisListDict.TryGetValue(identifier, out IRedisList redisList))
            {
                _logger.LogWarning("未能找到 Group为:'{0}',类型为:'{1}'的链表.", identifier.Group, identifier.DataType.FullName);

                var descriptor = new RedisListDescriptor()
                {
                    Group = group,
                    DataType = typeof(T)
                };

                redisList = Create<T>(descriptor);
                if (!_redisListDict.TryAdd(identifier, redisList))
                {
                    _logger.LogWarning("新增RedisList链表添加到集合失败!");
                }
            }

            if (redisList == null)
            {
                throw new NullReferenceException($"未能找到 Group为:'{group}',类型为:'{identifier.DataType.FullName}'的链表.");
            }

            return (IRedisList<T>)redisList;
        }


        private IRedisList<T> Create<T>(RedisListDescriptor descriptor) where T : IRedisListData
        {

            using (var scope = _provider.CreateScope())
            {
                var redisList = scope.ServiceProvider.GetService<IRedisList<T>>();

                var injectDescriptor = scope.ServiceProvider.GetService<RedisListDescriptor>();
                injectDescriptor.DataType = descriptor.DataType;
                injectDescriptor.Group = descriptor.Group;
                return redisList;
            }
        }


    }
}
