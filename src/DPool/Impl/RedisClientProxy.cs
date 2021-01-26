//using CSRedis;
//using Microsoft.Extensions.Options;

//namespace DPool.Impl
//{
//    /// <summary>Redis客户端代理
//    /// </summary>
//    public class RedisClientProxy : IRedisClientProxy
//    {
//        private readonly object _syncObject = new object();
//        private readonly DataPoolOption _option;
//        private CSRedisClient _client = null;
//        public RedisClientProxy(IOptions<DataPoolOption> option)
//        {
//            _option = option.Value;
//        }

//        /// <summary>获取客户端
//        /// </summary>
//        public CSRedisClient GetClient()
//        {
//            lock (_syncObject)
//            {
//                if (_client == null)
//                {
//                    _client = _option.GetRedisClient();
//                }
//            }
//            return _client;
//        }

//    }
//}
