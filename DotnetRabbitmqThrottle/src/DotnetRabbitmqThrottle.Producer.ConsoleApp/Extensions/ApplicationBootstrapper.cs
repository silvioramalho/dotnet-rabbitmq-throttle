using DotnetRabbitmqThrottle.Application;
using DotnetRabbitmqThrottle.Application.AutoMapper;
using DotnetRabbitmqThrottle.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace DotnetRabbitmqThrottle.Producer.ConsoleApp.Extensions
{
    public static class ApplicationBootstrapper
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, WorkerParams workerParams)
        {
            return services
                .AddAutoMapper(typeof(MessageMappingProfile))
                .AddSingleton((i) =>
                {
                    var factory = new ConnectionFactory()
                    {
                        Uri = new Uri(workerParams.RabbitMQConnectionString)
                    };

                    return factory.CreateConnection();
                })
                .AddSingleton<IChannelSetup, ChannelSetupService>()
                .AddSingleton<IProducerMessage, ProducerMessageService>();
        }
    }
}

