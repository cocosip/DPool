using DPool.GenericsPool;
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

        /// <summary>分组数据名(当名称设置的不同时,每个客户端从Redis取走数据后都将放在独立的一个组中)
        /// </summary>
        public string ProcessGroup { get; set; }

        /// <summary>数据类型
        /// </summary>
        public virtual Type DataType { get; set; }

        /// <summary>泛型数据池的类型
        /// </summary>
        public virtual Type GenericsDataPoolType { get; set; }

        /// <summary>泛型数据池配置信息类型
        /// </summary>
        public virtual Type GenericsDataPoolOptionType { get; set; }

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
