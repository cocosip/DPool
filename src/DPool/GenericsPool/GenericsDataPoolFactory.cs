using Microsoft.Extensions.DependencyInjection;
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

        public GenericsDataPoolFactory(ILogger<GenericsDataPoolFactory> logger, IServiceProvider provider)
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
                var option = scope.ServiceProvider.GetService(typeof(GenericsDataPoolOption<>));
                var convert = option as GenericsDataPoolOption;
                convert.DataType = descriptor.DataType;
                convert.Group = descriptor.Group;
                convert.IdSelector = descriptor.IdSelector;

                var genericsDataPool = scope.ServiceProvider.GetService(typeof(IGenericsDataPool<>));
                _logger.LogDebug("创建泛型数据池,Group:'{0}',DataType:'{1}'", descriptor.Group, descriptor.DataType.FullName);

                return genericsDataPool as IGenericsDataPool;
            }
        }

    }
}
