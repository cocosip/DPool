using System;

namespace DPool.RedisFx.List
{
    /// <summary>Redis链表
    /// </summary>
    public interface IRedisList
    {

    }

    /// <summary>Redis链表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRedisList<T> : IRedisList
    {
        /// <summary>获取注册信息
        /// </summary>
        RedisListDescriptor<T> GetDescriptor();

        /// <summary>添加数据元素
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        void Add(params T[] value);

        /// <summary>删除数据
        /// </summary>
        void Remove(params T[] value);

        /// <summary>根据Id集合删除数据
        /// </summary>
        void Remove(params string[] ids);

        /// <summary>获取指定个数的数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        T[] Get(int count);
    }
}
