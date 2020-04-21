namespace DPool.RedisFx.List
{
    /// <summary>所有的链表数据必须继承该接口
    /// </summary>
    public interface IRedisListData
    {
        /// <summary>筛选主键Id
        /// </summary>
        string SelectId();
    }
}
