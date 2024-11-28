using System;

namespace DPool.GenericsPool
{
    /// <summary>数据包装
    /// </summary>
    public class DataFuture<T>
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public DataFuture(string id, T data) : this(id, data, DateTime.Now)
        {

        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="createdOn"></param>
        public DataFuture(string id, T data, DateTime createdOn)
        {
            Id = id;
            Data = data;
            CreatedOn = createdOn;
        }

        /// <summary>
        /// IsTimeout
        /// </summary>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public bool IsTimeout(int timeoutSeconds)
        {
            return CreatedOn.AddSeconds(timeoutSeconds) < DateTime.Now;
        }
    }
}
