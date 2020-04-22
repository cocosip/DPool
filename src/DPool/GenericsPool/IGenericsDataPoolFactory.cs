namespace DPool.GenericsPool
{
    /// <summary>泛型数据池工厂
    /// </summary>
    public interface IGenericsDataPoolFactory
    {
        /// <summary>创建泛型数据池
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        IGenericsDataPool CreateGenericsDataPool(GenericsDataPoolDescriptor descriptor);
    }
}
