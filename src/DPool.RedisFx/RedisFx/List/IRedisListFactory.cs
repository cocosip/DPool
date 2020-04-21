namespace DPool.RedisFx.List
{
    /// <summary>Redis链表工厂
    /// </summary>
    public interface IRedisListFactory
    {
        /// <summary>获取链表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group"></param>
        /// <returns></returns>
        IRedisList<T> Get<T>(string group = "") where T : IRedisListData;
    }
}
