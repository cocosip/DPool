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
            return $"{DPoolConsts.DATA_PREFIX}.{group}.{type.FullName}";
        }

        /// <summary>生成数据锁Key
        /// </summary>
        public string GenerateDataLockName(string group, Type type)
        {
            return $"{DPoolConsts.DATA_LOCK_PREFIX}.{group}.{type.FullName}";
        }

        /// <summary>进行中的数据Key
        /// </summary>
        public string GenerateProcessDataIndexKey(string group, string processGroup, Type type)
        {
            return $"{DPoolConsts.PROCESS_DATA_INDEX_PREFIX}.{group}.{processGroup}.{type.FullName}";
        }


        /// <summary>进行中的数据Key
        /// </summary>
        public string GenerateProcessDataKey(string group, string processGroup, Type type, string id)
        {
            return $"{DPoolConsts.PROCESS_DATA_PREFIX}.{group}.{processGroup}.{type.FullName}.{id}";
        }



    }
}
