using DPool.GenericsPool;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace DPool
{
    /// <summary>数据池
    /// </summary>
    public class DataPool : IDataPool
    {
        public bool IsRunning { get { return _isRunning == 1; } }

        private int _isRunning = 0;

        private readonly ILogger _logger;
        private readonly DataPoolOption _option;
        private readonly IGenericsDataPoolFactory _genericsDataPoolFactory;

        private readonly ConcurrentDictionary<GenericsDataPoolIdentifier, IGenericsDataPool> _genericsDataPoolDict;

        /// <summary>Ctor
        /// </summary>
        public DataPool(ILogger<DataPool> logger, IOptions<DataPoolOption> option, IGenericsDataPoolFactory genericsDataPoolFactory)
        {
            _logger = logger;
            _option = option.Value;
            _genericsDataPoolFactory = genericsDataPoolFactory;

            _genericsDataPoolDict = new ConcurrentDictionary<GenericsDataPoolIdentifier, IGenericsDataPool>();
        }

        /// <summary>添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public long Write<T>(string group = "", params T[] value) where T : class, new()
        {
            var genericsDataPool = FindGenericsDataPool<T>(group);
            return genericsDataPool.Write(value);
        }

        /// <summary>获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public T[] Get<T>(int count, string group = "") where T : class, new()
        {
            var genericsDataPool = FindGenericsDataPool<T>(group);
            return genericsDataPool.Get(count);
        }

        /// <summary>归还数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group"></param>
        /// <param name="value"></param>
        public void Return<T>(string group = "", params T[] value) where T : class, new()
        {
            var genericsDataPool = FindGenericsDataPool<T>(group);
            genericsDataPool.Return(value);
        }

        /// <summary>释放数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Release<T>(string group = "", params T[] value) where T : class, new()
        {
            var genericsDataPool = FindGenericsDataPool<T>(group);
            genericsDataPool.Release(value);
        }

        /// <summary>运行
        /// </summary>
        public void Start()
        {
            if (IsRunning)
            {
                return;
            }

            Initialize();

            Interlocked.Exchange(ref _isRunning, 1);
        }

        /// <summary>停止
        /// </summary>
        public void Shutdown()
        {
            if (!IsRunning)
            {
                return;
            }

            //停止每个泛型数据池
            foreach (var kv in _genericsDataPoolDict)
            {
                kv.Value.Shutdown();
            }


            Interlocked.Exchange(ref _isRunning, 0);
        }



        #region Private method

        /// <summary>初始化
        /// </summary>
        private void Initialize()
        {
            foreach (var descriptor in _option.Descriptors)
            {
                var genericsDataPool = _genericsDataPoolFactory.CreateGenericsDataPool(descriptor);

                if (!_genericsDataPoolDict.TryAdd(genericsDataPool.Identifier, genericsDataPool))
                {
                    _logger.LogWarning("添加数据池失败,数据池信息:{0}", genericsDataPool.Identifier);
                }
                else
                {
                    //运行
                    genericsDataPool.Start();
                }
            }
        }


        /// <summary>查询泛型数据池
        /// </summary>
        private IGenericsDataPool<T> FindGenericsDataPool<T>(string group) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                group = _option.DefaultGroup;
            }
            var identifier = new GenericsDataPoolIdentifier(group, typeof(T));

            if (!_genericsDataPoolDict.TryGetValue(identifier, out IGenericsDataPool genericsDataPool))
            {
                throw new ArgumentException($"未找到任何相关的泛型数据池,数据池信息:{identifier}");
            }
            return genericsDataPool as IGenericsDataPool<T>;
        }

        #endregion



    }
}
