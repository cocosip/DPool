using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace DPool.RedisFx.List
{
    /// <summary>Redis Key生成器
    /// </summary>
    public class RedisListKeyGenerator : IRedisListKeyGenerator
    {
        private readonly ILogger _logger;
        private readonly RedisListOption _option;
        public RedisListKeyGenerator(ILogger<RedisListKeyGenerator> logger, IOptions<RedisListOption> option)
        {
            _logger = logger;
            _option = option.Value;
        }

        /// <summary>存放链表索引的Key
        /// </summary>
        /// <param name="type"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public string GenerateIndexKey(Type type, string group)
        {
            return $"{_option.RedisListIndexPrefix}.{group}.{type.FullName}";
        }

        /// <summary>链表存放每条数据Key
        /// </summary>
        /// <param name="type"></param>
        /// <param name="group"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GenerateDataKey(Type type, string group, string id)
        {
            return $"{_option.RedisListDataPrefix}.{group}.{type.FullName}.{id}";
        }

    }
}
