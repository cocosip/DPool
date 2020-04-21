namespace DPool.RedisFx.List
{
    /// <summary>配置信息
    /// </summary>
    public class RedisListOption
    {
        /// <summary>索引前缀
        /// </summary>
        public string RedisListIndexPrefix { get; set; } = RedisFxConsts.REDIS_LIST_INDEX_PREFIX;

        /// <summary>数据前缀
        /// </summary>
        public string RedisListDataPrefix { get; set; } = RedisFxConsts.REDIS_LIST_DATA_PREFIX;

        /// <summary>默认分组
        /// </summary>
        public string DefaultGroup { get; set; } = RedisFxConsts.DEFAULT_GROUP;

        /// <summary>Ctor
        /// </summary>
        public RedisListOption()
        {

        }



    }
}
