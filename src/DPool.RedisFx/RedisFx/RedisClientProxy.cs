using CSRedis;
using Microsoft.Extensions.Options;

namespace DPool.RedisFx
{
    public class RedisClientProxy : IRedisClientProxy
    {
        private readonly RedisFxOption _option;

        public RedisClientProxy(IOptions<RedisFxOption> option)
        {
            _option = option.Value;
        }

        /// <summary>获取客户端
        /// </summary>
        public CSRedisClient GetClient()
        {
            return _option.Client;
        }
    }
}
