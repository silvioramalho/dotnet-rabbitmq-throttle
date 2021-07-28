using RabbitMQ.Client;

namespace DotnetRabbitmqThrottle.Application
{
    public interface IChannelSetup
    {
        IModel CreateChannel(string queueName);
    }
}