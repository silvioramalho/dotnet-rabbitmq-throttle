using System;
using DotnetRabbitmqThrottle.Application;
using DotnetRabbitmqThrottle.Application.AutoMapper;
using DotnetRabbitmqThrottle.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace DotnetRabbitmqThrottle.Producer.API.Extensions
{
    public static class ApplicationBootstrapper
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddAutoMapper(typeof(MessageMappingProfile))
                .AddSingleton((i) =>
                {
                    var factory = new ConnectionFactory()
                    {
                        Uri = new Uri(configuration.GetConnectionString("RabbitMQ"))
                    };

                    return factory.CreateConnection();
                })
                .AddScoped<IChannelSetup, ChannelSetupService>()
                .AddScoped<IProducerMessage, ProducerMessageService>();
        }
    }
}