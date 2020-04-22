using System;

namespace DPool.GenericsPool
{
    /// <summary>数据包装
    /// </summary>
    public class DataFuture<T>
    {
        /// <summary>Id
        /// </summary>
        public string Id { get; set; }

        public T Data { get; set; }

        public DateTime CreatedOn { get; set; }


        public DataFuture(string id, T data) : this(id, data, DateTime.Now)
        {

        }

        public DataFuture(string id, T data, DateTime createdOn)
        {
            Id = id;
            Data = data;
            CreatedOn = createdOn;
        }

        public bool IsTimeout(int timeoutSeconds)
        {
            return CreatedOn.AddSeconds(timeoutSeconds) < DateTime.Now;
        }
    }
}
