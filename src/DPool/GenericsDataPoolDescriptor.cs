using System;

namespace DPool
{
    /// <summary>泛型数据池描述信息
    /// </summary>
    public class GenericsDataPoolDescriptor
    {
        /// <summary>组信息
        /// </summary>
        public string Group { get; set; }

        /// <summary>数据类型
        /// </summary>
        public virtual Type DataType { get; set; }

        /// <summary>Id选择器
        /// </summary>
        public virtual Delegate IdSelector { get; set; }
    }

    /// <summary>泛型数据池描述信息
    /// </summary>
    public class GenericsDataPoolDescriptor<T> : GenericsDataPoolDescriptor
    {
        /// <summary>数据类型
        /// </summary>
        public override Type DataType { get; set; } = typeof(T);

 
    }
}
