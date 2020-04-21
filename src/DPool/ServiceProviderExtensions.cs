﻿using Microsoft.Extensions.DependencyInjection;
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
        public static IServiceProvider ConfigureDataPool(this IServiceProvider provider, Action<DataPoolOption> configure = null)
        {
            if (configure != null)
            {
                var option = provider.GetService<IOptions<DataPoolOption>>().Value;
                configure(option);
            }

            return provider;
        }
    }
}
