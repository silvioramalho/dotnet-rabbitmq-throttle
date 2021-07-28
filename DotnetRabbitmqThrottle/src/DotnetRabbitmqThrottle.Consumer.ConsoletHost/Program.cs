using System;
using DotnetRabbitmqThrottle.Consumer.ConsoletHost.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DotnetRabbitmqThrottle.Consumer.ConsoletHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(
                "RabbitMQ Queue Consumer - Started");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var workerParams = new WorkerParams()
                    {
                        RabbitMQConnectionString = hostContext.Configuration.GetConnectionString("RabbitMQ"),
                        RedisConnectionString = hostContext.Configuration.GetConnectionString("Redis"),
                        QueueName = args[0] ?? hostContext.Configuration["DefaultQueueName"]
                };
                    services.AddSingleton<WorkerParams>(workerParams);
                    services.AddApplicationServices(workerParams);
                    services.AddHostedService<Worker>();
                });
    }
}