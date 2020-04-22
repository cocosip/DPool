namespace DPool
{
    /// <summary>数据池
    /// </summary>
    public interface IDataPool
    {
        /// <summary>运行
        /// </summary>
        void Start();

        /// <summary>停止
        /// </summary>
        void Shutdown();

        /// <summary>添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        long Write<T>(string group = "", params T[] value) where T : class, new();

        /// <summary>获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        T[] Get<T>(int count, string group = "") where T : class, new();

        /// <summary>归还数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group"></param>
        /// <param name="value"></param>
        void Return<T>(string group = "", params T[] value) where T : class, new();

        /// <summary>释放数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void Release<T>(string group = "", params T[] value) where T : class, new();

    }

}
