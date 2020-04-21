using System;

namespace DPool.RedisFx.List
{
    /// <summary>Redis Key生成器
    /// </summary>
    public interface IRedisListKeyGenerator
    {

        /// <summary>存放链表索引的Key
        /// </summary>
        /// <param name="type"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        string GenerateIndexKey(Type type, string group);


        /// <summary>链表存放每条数据Key
        /// </summary>
        /// <param name="type"></param>
        /// <param name="group"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        string GenerateDataKey(Type type, string group, string id);
    }
}
