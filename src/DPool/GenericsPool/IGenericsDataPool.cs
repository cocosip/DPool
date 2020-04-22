namespace DPool.GenericsPool
{
    /// <summary>泛型数据池
    /// </summary>
    public interface IGenericsDataPool
    {

        /// <summary>是否正在运行
        /// </summary>
        bool IsRunning { get; }

        /// <summary>身份标志
        /// </summary>
        GenericsDataPoolIdentifier Identifier { get; }

        /// <summary>运行
        /// </summary>
        void Start();

        /// <summary>停止
        /// </summary>
        void Shutdown();
    }

    /// <summary>泛型数据池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericsDataPool<T> : IGenericsDataPool where T : class, new()
    {
        /// <summary>添加数据
        /// </summary>
        /// <param name="value"></param>
        long Write(T[] value);

        /// <summary>获取指定数量的数据
        /// </summary>
        /// <param name="count"></param>
        T[] Get(int count);

        /// <summary>归还数据,将数据归还到集合中
        /// </summary>
        /// <param name="value"></param>
        void Return(params T[] value);

        /// <summary>删除数据,从取走的数据中删除
        /// </summary>
        /// <param name="value"></param>
        void Release(params T[] value);
    }
}
