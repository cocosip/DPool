using System;

namespace DPool.GenericsPool
{
    /// <summary>泛型数据池配置信息
    /// </summary>
    public class GenericsDataPoolOptions
    {
        /// <summary>组信息
        /// </summary>
        public string Group { get; set; }

        /// <summary>数据类型
        /// </summary>
        public virtual Type DataType { get; set; }

        /// <summary>泛型数据池的类型
        /// </summary>
        public virtual Type GenericsDataPoolType { get; set; }

        /// <summary>泛型数据池配置信息类型
        /// </summary>
        public virtual Type GenericsDataPoolOptionType { get; set; }

        /// <summary>进行中数据的前缀
        /// </summary>
        public string ProcessGroup { get; set; } = DPoolConsts.PROCESS_DATA_PREFIX;

        /// <summary>Id选择器
        /// </summary>
        public virtual Delegate IdSelector { get; set; }

    }

    /// <summary>泛型数据池配置信息
    /// </summary>
    public class GenericsDataPoolOptions<T> : GenericsDataPoolOptions
    {
        /// <summary>数据类型
        /// </summary>
        public override Type DataType { get; set; } = typeof(T);

        /// <summary>泛型数据库配置信息类型
        /// </summary>
        public override Type GenericsDataPoolOptionType { get; set; } = typeof(GenericsDataPoolOptions<T>);

    }
}
