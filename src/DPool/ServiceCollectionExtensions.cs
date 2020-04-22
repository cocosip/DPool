using DPool.GenericsPool;
using DPool.Impl;
using DPool.Scheduling;
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
        public static IServiceCollection AddDataPool(this IServiceCollection services, Action<DataPoolOption> configure)
        {
            services
                .Configure<DataPoolOption>(configure)
                .AddSingleton<IScheduleService, ScheduleService>()
                .AddSingleton<DataPool>()
                .AddSingleton<IGenericsDataPoolFactory, GenericsDataPoolFactory>()
                .AddSingleton<IDPoolKeyGenerator, DPoolKeyGenerator>()
                .AddScoped(typeof(GenericsDataPool<>))
                .AddScoped(typeof(GenericsDataPoolOption<>))
                ;

            return services;
        }
    }
}
