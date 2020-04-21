using System;
using System.Collections.Generic;
using System.Text;

namespace DPool.RedisFx.List
{
    /// <summary>Redis链表身份标志
    /// </summary>
    public class RedisListIdentifier : IEquatable<RedisListIdentifier>
    {
        /// <summary>分组
        /// </summary>
        public string Group { get; set; }

        /// <summary>数据类型
        /// </summary>
        public Type DataType { get; set; }

        public bool Equals(RedisListIdentifier other)
        {
            if (other is null)
            {
                return false;
            }
            return other is RedisListIdentifier && Equals((RedisListIdentifier)other);
        }


        /// <summary>重写相等方法
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }
            return obj is RedisListIdentifier && Equals((RedisListIdentifier)obj);
        }

        /// <summary>重写获取HashCode方法
        /// </summary>
        public override int GetHashCode()
        {
            return StringComparer.InvariantCulture.GetHashCode(Group) & DataType.GetHashCode();
        }

    }
}
