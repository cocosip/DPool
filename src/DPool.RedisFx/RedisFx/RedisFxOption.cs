using CSRedis;
using System;

namespace DPool.RedisFx
{
    /// <summary>Redis扩展配置
    /// </summary>
    public class RedisFxOption
    {
        /// <summary>Redis客户端
        /// </summary>
        public CSRedisClient Client { get; set; }
    }
}
