using System;

namespace DPool.GenericsPool
{
    /// <summary>泛型数据池标志
    /// </summary>
    public class GenericsDataPoolIdentifier : IEquatable<GenericsDataPoolIdentifier>
    {
        /// <summary>分组
        /// </summary>
        public string Group { get; set; }

        /// <summary>数据类型
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>Ctor
        /// </summary>
        public GenericsDataPoolIdentifier()
        {

        }

        /// <summary>Ctor
        /// </summary>
        public GenericsDataPoolIdentifier(string group, Type dataType)
        {
            Group = group;
            DataType = dataType;
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(GenericsDataPoolIdentifier other)
        {
            if (other is null)
            {
                return false;
            }

            return Group.Equals(other.Group, StringComparison.OrdinalIgnoreCase) && DataType == other.DataType;
        }

        /// <summary>
        /// 重写相等方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }
            return obj is GenericsDataPoolIdentifier && Equals((GenericsDataPoolIdentifier)obj);
        }



        /// <summary>重写获取HashCode方法
        /// </summary>
        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Group) & DataType.FullName.GetHashCode();
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[Group:{Group}-Type:{DataType.FullName}]";
        }


    }
}
