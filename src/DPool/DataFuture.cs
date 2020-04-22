using DPool.RedisFx.List;
using System;

namespace DPool
{
    /// <summary>数据包装
    /// </summary>
    public class DataFuture<T>
    {
        public T Data { get; set; }

        public DateTime CreatedOn { get; set; }


        public DataFuture(T data) : this(data, DateTime.Now)
        {

        }

        public DataFuture(T data, DateTime createdOn)
        {
            Data = data;
            CreatedOn = createdOn;
        }

        public bool IsExpired(int expireSeconds)
        {
            return CreatedOn.AddSeconds(expireSeconds) < DateTime.Now;
        }
    }
}
