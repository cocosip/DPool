using CSRedis;

namespace DPool.RedisFx
{
    /// <summary>Redis客户端代理
    /// </summary>
    public interface IRedisClientProxy
    {
        /// <summary>获取客户端
        /// </summary>
        CSRedisClient GetClient();
    }
}
