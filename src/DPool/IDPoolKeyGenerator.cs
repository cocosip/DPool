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
        string GenerateDataLockKey(string group, Type type);
    }
}
