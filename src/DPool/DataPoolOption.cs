namespace DPool
{
    /// <summary>配置信息
    /// </summary>
    public class DataPoolOption
    {

        /// <summary>数据前缀
        /// </summary>
        public string DataPrefix { get; set; } = DPoolConsts.DATA_PREFIX;

        /// <summary>数据锁前缀
        /// </summary>
        public string DataLockPrefix { get; set; } = DPoolConsts.DATA_LOCK_PREFIX;

        /// <summary>默认分组
        /// </summary>
        public string DefaultGroup { get; set; } = DPoolConsts.DEFAULT_GROUP;

        /// <summary>数据锁定的秒数
        /// </summary>
        public int DataLockSeconds { get; set; } = 2;

        /// <summary>查询超时的数据时间间隔(ms)
        /// </summary>
        public int ScanTimeoutDataInterval { get; set; } = 5000;

        /// <summary>多久的数据算超时
        /// </summary>
        public int DateTimeoutSeconds { get; set; } = 30;

    }
}
