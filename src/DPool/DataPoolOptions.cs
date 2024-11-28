using DPool.GenericsPool;
using FreeRedis;
using System;
using System.Collections.Generic;

namespace DPool
{
    /// <summary>配置信息
    /// </summary>
    public class DataPoolOptions
    {
        /// <summary>如果没有分组名的时候,默认用该分组名
        /// </summary>
        public string DefaultGroup { get; set; } = DPoolConsts.DEFAULT_GROUP;

        /// <summary>处理中数据分组
        /// </summary>
        public string DefaultProcessGroup { get; set; } = DPoolConsts.PROCESS_GROUP;

        /// <summary>数据锁定的秒数
        /// </summary>
        public int DataLockSeconds { get; set; } = 2;

        /// <summary>查询超时的数据时间间隔(ms)
        /// </summary>
        public int ScanTimeoutDataInterval { get; set; } = 5000;

        /// <summary>多久的数据算超时
        /// </summary>
        public int DataTimeoutSeconds { get; set; } = 30;

        /// <summary>泛型数据池的描述信息
        /// </summary>
        public List<GenericsDataPoolDescriptor> Descriptors { get; set; } = [];

        /// <summary>
        /// RedisClient的配置
        /// </summary>
        public Func<IRedisClient> RedisClientFunc { get; set; }


        /// <summary>Ctor
        /// </summary>
        public DataPoolOptions()
        {
        }

        /// <summary>添加泛型数据池
        /// </summary>
        public DataPoolOptions AddDescriptor<T>(GenericsDataPoolDescriptor<T> descriptor) where T : class, new()
        {
            Descriptors.Add(descriptor);
            return this;
        }

        /// <summary>添加泛型数据池
        /// </summary>
        public DataPoolOptions AddDescriptor<T>(Func<T, string> idSelector, string group = "", string processGroup = "") where T : class, new()
        {
            var descriptor = new GenericsDataPoolDescriptor<T>()
            {
                Group = group,
                DataType = typeof(T),
                GenericsDataPoolType = typeof(IGenericsDataPool<T>),
                GenericsDataPoolOptionType = typeof(GenericsDataPoolOptions<T>),
                ProcessGroup = processGroup,
                IdSelector = idSelector
            };
            return AddDescriptor<T>(descriptor);
        }

    }
}
