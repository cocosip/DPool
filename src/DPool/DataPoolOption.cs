using CSRedis;
using System;
using System.Collections.Generic;

namespace DPool
{
    /// <summary>配置信息
    /// </summary>
    public class DataPoolOption
    {
        /// <summary>默认分组
        /// </summary>
        public string DefaultGroup { get; set; } = DPoolConsts.DEFAULT_GROUP;

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
        public List<GenericsDataPoolDescriptor> Descriptors { get; set; } = new List<GenericsDataPoolDescriptor>();

        /// <summary>获取客户端的委托
        /// </summary>
        public Func<CSRedisClient> GetRedisClient { get; set; }

        /// <summary>Ctor
        /// </summary>
        public DataPoolOption()
        {

        }

        /// <summary>添加泛型数据池
        /// </summary>
        public DataPoolOption AddDescriptor<T>(GenericsDataPoolDescriptor<T> descriptor)
        {
            Descriptors.Add(descriptor);
            return this;
        }

        /// <summary>添加泛型数据池
        /// </summary>
        public DataPoolOption AddDescriptor<T>(Func<T, string> idSelector, string group = DPoolConsts.DEFAULT_GROUP)
        {
            var descriptor = new GenericsDataPoolDescriptor<T>()
            {
                Group = group,
                DataType = typeof(T),
                IdSelector = idSelector
            };
            return AddDescriptor<T>(descriptor);
        }

    }
}
