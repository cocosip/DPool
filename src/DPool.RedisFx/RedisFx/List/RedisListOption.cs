using System;
using System.Collections.Generic;

namespace DPool.RedisFx.List
{
    /// <summary>配置信息
    /// </summary>
    public class RedisListOption
    {
        /// <summary>索引前缀
        /// </summary>
        public string RedisListIndexPrefix { get; set; } = RedisFxConsts.REDIS_LIST_INDEX_PREFIX;

        /// <summary>数据前缀
        /// </summary>
        public string RedisListDataPrefix { get; set; } = RedisFxConsts.REDIS_LIST_DATA_PREFIX;

        /// <summary>默认分组
        /// </summary>
        public string DefaultGroup { get; set; } = RedisFxConsts.DEFAULT_GROUP;

        /// <summary>注册信息
        /// </summary>
        public List<RedisListDescriptor> Descriptors { get; set; } = new List<RedisListDescriptor>();


        /// <summary>Ctor
        /// </summary>
        public RedisListOption()
        {

        }

        public RedisListOption AddDescriptor<T>(string group, Func<T, string> idSelector)
        {
            var descriptor = new RedisListDescriptor<T>()
            {
                DataType = typeof(T),
                Group = group,
                IdSelector = idSelector
            };
            return AddDescriptor(descriptor);
        }

        /// <summary>添加注册信息
        /// </summary>
        public RedisListOption AddDescriptor<T>(RedisListDescriptor<T> descriptor)
        {
            Descriptors.Add(descriptor);
            return this;
        }



    }
}
