﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace DPool.GenericsPool
{
    /// <summary>泛型数据池工厂
    /// </summary>
    public class GenericsDataPoolFactory : IGenericsDataPoolFactory
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _provider;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="provider"></param>
        public GenericsDataPoolFactory(
            ILogger<GenericsDataPoolFactory> logger, 
            IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        /// <summary>创建泛型数据池
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public IGenericsDataPool CreateGenericsDataPool(GenericsDataPoolDescriptor descriptor)
        {
            using (var scope = _provider.CreateScope())
            {
                var option = scope.ServiceProvider.GetService(descriptor.GenericsDataPoolOptionType);
                var convert = (GenericsDataPoolOptions)option;

                convert.DataType = descriptor.DataType;
                convert.GenericsDataPoolType = descriptor.GenericsDataPoolType;
                convert.GenericsDataPoolOptionType = descriptor.GenericsDataPoolOptionType;

                convert.Group = descriptor.Group;
                convert.ProcessGroup = descriptor.ProcessGroup;
                convert.IdSelector = descriptor.IdSelector;

                var genericsDataPool = scope.ServiceProvider.GetService(descriptor.GenericsDataPoolType);
                _logger.LogDebug("创建泛型数据池,Group:'{0}',DataType:'{1}'", descriptor.Group, descriptor.DataType.FullName);

                return genericsDataPool as IGenericsDataPool;
            }
        }

    }
}
