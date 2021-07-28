using System;
using DotnetRabbitmqThrottle.Application;
using DotnetRabbitmqThrottle.Application.AutoMapper;
using DotnetRabbitmqThrottle.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using StackExchange.Redis;

namespace DotnetRabbitmqThrottle.Consumer.ConsoletHost.Extensions
{
    public static class ApplicationBootstrapper
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, WorkerParams workerParams)
        {
            return services
                .AddAutoMapper(typeof(MessageMappingProfile))
                .AddSingleton<IConnectionMultiplexer>(i => ConnectionMultiplexer.Connect(workerParams.RedisConnectionString))
                .AddSingleton<ITokenBucketThrottling, TokenBucketThrottlingService>()
                .AddSingleton((i) =>
                {
                    var factory = new ConnectionFactory()
                    {
                        Uri = new Uri(workerParams.RabbitMQConnectionString)
                    };

                    return factory.CreateConnection();
                })
                .AddSingleton((s) =>
                {
                    var connection = s.GetService<IConnection>();
                    return new ChannelSetupService(connection).CreateChannel(workerParams.QueueName);
                })
                .AddSingleton<IConsumerMessage, ConsumerMessageService>();
        }
    }
}