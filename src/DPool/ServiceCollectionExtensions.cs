using DPool.GenericsPool;
using DPool.Impl;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DPool
{
    /// <summary>依赖注入扩展
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>添加数据池
        /// </summary>
        public static IServiceCollection AddDPool(this IServiceCollection services, Action<DataPoolOptions> configure = null)
        {
            configure ??= new Action<DataPoolOptions>(c => { });

            services
                .Configure<DataPoolOptions>(configure)
                .AddSingleton<IDataPool, DataPool>()
                .AddSingleton<IGenericsDataPoolFactory, GenericsDataPoolFactory>()
                .AddSingleton<IDPoolKeyGenerator, DPoolKeyGenerator>()
                .AddSingleton<IRedisClientProxy, RedisClientProxy>()
                .AddScoped(typeof(IGenericsDataPool<>), typeof(GenericsDataPool<>))
                .AddScoped(typeof(GenericsDataPoolOptions<>))
                ;

            return services;
        }
    }
}
