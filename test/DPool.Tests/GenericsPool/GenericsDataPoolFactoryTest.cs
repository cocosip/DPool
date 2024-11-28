using DPool.GenericsPool;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DPool.Tests.GenericsPool
{
    public class GenericsDataPoolFactoryTest
    {
        private readonly IServiceProvider _provider;
        public GenericsDataPoolFactoryTest()
        {
            IServiceCollection services = new ServiceCollection();
            services
                .AddLogging()
                .AddDPool(c =>
            {

            });
            _provider = services.BuildServiceProvider();
        }

        [Fact]
        public void CreateGenericsDataPool_Test()
        {
            var factory = _provider.GetService<IGenericsDataPoolFactory>();
            var genericsDataPool = factory.CreateGenericsDataPool(new GenericsDataPoolDescriptor()
            {
                Group = "Group1",
                DataType = this.GetType(),
                GenericsDataPoolType = typeof(IGenericsDataPool<GenericsDataPoolFactoryTest>),
                GenericsDataPoolOptionType = typeof(GenericsDataPoolOptions<GenericsDataPoolFactoryTest>),
                ProcessGroup = "Process1",
                IdSelector = new Func<GenericsDataPoolFactoryTest, string>(t => { return "1"; })
            });

            Assert.Equal("Group1", genericsDataPool.Identifier.Group);
            Assert.Equal(this.GetType(), genericsDataPool.Identifier.DataType);

        }

    }
}
