using Microsoft.Extensions.DependencyInjection;
using System;

namespace DPool.RedisFx
{
    /// <summary>依赖注入扩展
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>添加Redis链表
        /// </summary>
        public static IServiceCollection AddRedisFx(this IServiceCollection services, Action<RedisFxOption> configure)
        {
            services
                .Configure<RedisFxOption>(configure)
                .AddSingleton<IRedisClientProxy, RedisClientProxy>()
                ;
            return services;
        }

    }
}
