using System;

namespace DPool
{
    /// <summary>数据Key生成器
    /// </summary>
    public interface IDPoolKeyGenerator
    {
        /// <summary>生成数据Key
        /// </summary>
        string GenerateDataKey(string group, Type type);

        /// <summary>生成数据锁Key
        /// </summary>
        string GenerateDataLockName(string group, Type type);

        /// <summary>进行中的数据Key
        /// </summary>
        string GenerateProcessDataIndexKey(string group, string processGroup, Type type);

        /// <summary>进行中的数据Key
        /// </summary>
        string GenerateProcessDataKey(string group, string processGroup, Type type, string id);
    }
}
