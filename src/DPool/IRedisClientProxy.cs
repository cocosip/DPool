using FreeRedis;

namespace DPool
{

    /// <summary>Redis客户端代理
    /// </summary>
    public interface IRedisClientProxy
    {
        /// <summary>获取客户端
        /// </summary>
        IRedisClient GetClient();
    }
}
