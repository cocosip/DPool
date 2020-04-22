using System;

namespace DPool.GenericsPool
{
    /// <summary>泛型数据池配置信息
    /// </summary>
    public class GenericsDataPoolOption
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

    /// <summary>泛型数据池配置信息
    /// </summary>
    public class GenericsDataPoolOption<T> : GenericsDataPoolOption
    {
        /// <summary>数据类型
        /// </summary>
        public override Type DataType { get; set; } = typeof(T);

    }
}
