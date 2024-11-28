using FreeRedis;
using Microsoft.Extensions.Options;

namespace DPool.Impl
{
    /// <summary>Redis客户端代理
    /// </summary>
    public class RedisClientProxy : IRedisClientProxy
    {
        private readonly object _syncObject = new object();
        private readonly DataPoolOptions _options;
        private IRedisClient _client = null;
        public RedisClientProxy(IOptions<DataPoolOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>获取客户端
        /// </summary>
        public IRedisClient GetClient()
        {
            lock (_syncObject)
            {
                if (_client == null)
                {
                    _client = _options.RedisClientFunc();
                }
            }
            return _client;
        }

    }
}
