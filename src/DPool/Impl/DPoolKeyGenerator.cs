using Microsoft.Extensions.Options;
using System;

namespace DPool.Impl
{
    /// <summary>数据Key生成器
    /// </summary>
    public class DPoolKeyGenerator : IDPoolKeyGenerator
    {
        private readonly DataPoolOption _option;
        public DPoolKeyGenerator(IOptions<DataPoolOption> option)
        {
            _option = option.Value;
        }

        /// <summary>生成数据Key
        /// </summary>
        public string GenerateDataKey(string group, Type type)
        {
            return $"{_option.DataPrefix}.{group}.{type.FullName}";
        }

        /// <summary>生成数据锁Key
        /// </summary>
        public string GenerateDataLockKey(string group, Type type)
        {
            return $"{_option.DataLockPrefix}.{group}.{type.FullName}";
        }

    }
}
