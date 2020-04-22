namespace DPool
{
    /// <summary>常量
    /// </summary>
    public static class DPoolConsts
    {
        /// <summary>数据前缀
        /// </summary>
        public const string DATA_PREFIX = "DPool.Data";

        /// <summary>数据锁的前缀
        /// </summary>
        public const string DATA_LOCK_PREFIX = "DPool.DataLock";

        /// <summary>进行中的数据索引
        /// </summary>
        public const string PROCESS_DATA_INDEX_PREFIX = "DPool.ProcessDataIndex";

        /// <summary>进行中的数据
        /// </summary>
        public const string PROCESS_DATA_PREFIX = "DPool.ProcessData";

        /// <summary>默认分组名
        /// </summary>
        public const string DEFAULT_GROUP = "Default";

        /// <summary>进行中数据分组
        /// </summary>
        public const string PROCESS_GROUP = "Process1";

        /// <summary>超时数据
        /// </summary>
        public const string SCAN_TIMEOUT_DATA_NAME = "DPool.ScanTimeoutData";

        /// <summary>进行中数据时间的字段
        /// </summary>
        public const string PROCESS_DATA_DATETIME_FIELD = "DateTime";

        /// <summary>进行中的数据数据字段
        /// </summary>
        public const string PROCESS_DATA_DATA_FIELD = "Data";
    }
}
