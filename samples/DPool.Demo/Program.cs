using CSRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
                RedisHelper.Initialization(new CSRedisClient("mymaster,password=123456,prefix=my_", new string[] { "192.168.0.38:26379", "192.168.0.39:26379", "192.168.0.87:26379" }, false));


                //RedisHelper.Initialization(new CSRedisClient("192.168.0.38:6379,password=123456,prefix=my_"));
                //RedisHelper.Set("ss", "123");
                //Console.WriteLine(RedisHelper.Get("ss"));

                services
                    .AddLogging(l =>
                    {
                        l.SetMinimumLevel(LogLevel.Trace);
                        l.AddConsole();
                    })
                    .AddDPool(c =>
                    {
                        c.AddDescriptor<TestUser>(x => x.Id);
                    });

                var provider = services.BuildServiceProvider();
                provider.ConfigureDPool();

                var dataPool = provider.GetService<IDataPool>();

                // var d1 = dataPool.Get<TestUser>(10);

                for (int i = 0; i < 100; i++)
                {
                    dataPool.Write<TestUser>("", new TestUser()
                    {
                        Id = "id_" + i,
                        Name = "zhangsan" + i
                    });
                }

                int count = 1;
                while (count > 0)
                {
                    var value = dataPool.Get<TestUser>(20);
                    if (value != null)
                    {
                        count = value.Length;
                        dataPool.Release<TestUser>("", value);
                    }
                    else
                    {
                        value = new TestUser[0];
                        count = 0;
                    }
                    Console.WriteLine("本次获取数据:{0},id集合为:[{1}]", value.Length, string.Join(",", value.Select(x => x.Id)));
                }



            }, TaskCreationOptions.LongRunning);
        }


        class TestUser
        {
            public string Id { get; set; }

            public string Name { get; set; }
        }
    }
}
