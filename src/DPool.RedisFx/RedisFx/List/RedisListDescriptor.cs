using System;

namespace DPool.RedisFx.List
{
    /// <summary>注册信息
    /// </summary>
    public class RedisListDescriptor
    {
        /// <summary>分组
        /// </summary>
        public string Group { get; set; }

        /// <summary>数据类型
        /// </summary>
        public Type DataType { get; set; }
    }
}
