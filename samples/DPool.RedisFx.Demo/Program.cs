using DPool.RedisFx.List;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DPool.RedisFx.Demo
{
    class Program
    {
        static IServiceProvider ServiceProvider = null;
        static void Main(string[] args)
        {
            Console.WriteLine("Redis自定义集合测试");

            IServiceCollection services = new ServiceCollection();
            services
                .AddLogging()
                .AddRedisFx(c =>
                {
                    c.Client = new CSRedis.CSRedisClient("192.168.0.38:6379,password=123456,prefix=my_");

                })
                .AddRedisList(c =>
                {
                    c.RedisListIndexPrefix = "RedisListIndex";
                    c.RedisListDataPrefix = "RedisListData";
                    c.AddDescriptor<TestUser>(RedisFxConsts.DEFAULT_GROUP, x => x.Id);
                })
                ;

            ServiceProvider = services.BuildServiceProvider();

            ServiceProvider
                .ConfigureRedisFx();


            //运行
            Run();

            Console.ReadLine();
        }


        public static void Run()
        {
            Task.Factory.StartNew(() =>
            {
                var factory = ServiceProvider.GetService<IRedisListFactory>();
                var list = factory.Get<TestUser>();

                while (true)
                {
                    var users = new List<TestUser>();
                    for (int i = 0; i < 50; i++)
                    {

                        var user = new TestUser()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "Test",
                            Age = 20
                        };
                        users.Add(user);
                    }
                    list.Add(users.ToArray());

                    var testUsers1 = list.Get(20);
                    Console.WriteLine("获取20条用户,返回:{0} 条", testUsers1.Length);

                    Thread.Sleep(10);

                    list.Remove(testUsers1);

                    var testUsers2 = list.Get(3);
                    Console.WriteLine("获取3条用户,返回:{0} 条", testUsers2.Length);
                    if (testUsers2.Any(x => x == null))
                    {
                        Console.WriteLine("用户存在NULL");
                    }

                    list.Remove(testUsers2);

                    Thread.Sleep(1000);
                }


            });
        }

    }


    public class TestUser
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string SelectId()
        {
            return Id;
        }
    }
}
