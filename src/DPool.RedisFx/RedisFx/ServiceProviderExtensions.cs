using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace DPool.RedisFx
{
    /// <summary>依赖注入扩展
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>配置
        /// </summary>
        public static IServiceProvider ConfigureRedisFx(this IServiceProvider provider, Action<RedisFxOption> configure = null)
        {
            if (configure != null)
            {
                var option = provider.GetService<IOptions<RedisFxOption>>().Value;
                configure(option);
            }

            return provider;
        }
    }
}
