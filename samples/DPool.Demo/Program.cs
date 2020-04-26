using CSRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DPool.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("DPool测试程序...");

            RunTest();

            Console.ReadLine();
        }


        static Task RunTest()
        {
            return Task.Factory.StartNew(() =>
            {

                IServiceCollection services = new ServiceCollection();
                services
                    .AddLogging(l =>
                    {
                        l.SetMinimumLevel(LogLevel.Trace);
                        l.AddConsole();
                    })
                    .AddDPool(c =>
                    {
                        c.AddDescriptor<TestUser>(x => x.Id.ToString());
                        c.GetRedisClient = () => new CSRedis.CSRedisClient("192.168.0.6:6379,password=123456,prefix=my_");

                    });

                var provider = services.BuildServiceProvider();
                provider.ConfigureDPool();

                var dataPool = provider.GetService<IDataPool>();

                for (int i = 0; i < 100; i++)
                {
                    dataPool.Write<TestUser>("", new TestUser()
                    {
                        Id = i,
                        Name = "zhangsan" + i
                    });
                }

                dataPool.Get<TestUser>(20);

            }, TaskCreationOptions.LongRunning);
        }


        class TestUser
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
