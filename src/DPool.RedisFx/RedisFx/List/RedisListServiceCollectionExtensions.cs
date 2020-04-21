using Microsoft.Extensions.DependencyInjection;
using System;

namespace DPool.RedisFx.List
{
    /// <summary>依赖注入扩展
    /// </summary>
    public static class RedisListServiceCollectionExtensions
    {
        /// <summary>添加Redis链表
        /// </summary>
        public static IServiceCollection AddRedisList(this IServiceCollection services, Action<RedisListOption> configure = null)
        {
            if (configure == null)
            {
                configure = c => { };
            }

            services
                .Configure<RedisListOption>(configure)
                .AddSingleton<IRedisListFactory, RedisListFactory>()
                .AddSingleton<IRedisListKeyGenerator, RedisListKeyGenerator>()
                .AddScoped(typeof(IRedisList<>), typeof(RedisList<>))
                .AddScoped(typeof(RedisListDescriptor))
                ;

            return services;
        }
    }
}
