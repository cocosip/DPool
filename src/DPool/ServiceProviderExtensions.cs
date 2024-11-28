using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace DPool
{
    /// <summary>依赖注入扩展
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>配置数据池
        /// </summary>
        public static IServiceProvider ConfigureDPool(this IServiceProvider provider, Action<DataPoolOptions> configure = null)
        {
            if (configure != null)
            {
                var option = provider.GetService<IOptions<DataPoolOptions>>().Value;
                configure(option);
            }
            var dataPool = provider.GetService<IDataPool>();
            dataPool.Start();
            return provider;
        }
    }
}
