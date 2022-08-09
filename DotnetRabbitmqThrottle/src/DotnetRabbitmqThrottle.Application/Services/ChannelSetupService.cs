using System.Collections.Generic;
using RabbitMQ.Client;

namespace DotnetRabbitmqThrottle.Application.Services
{
    public class ChannelSetupService : IChannelSetup
    {
        private readonly IConnection _connection;
        private IModel _channel;

        public ChannelSetupService(
            IConnection connection)
        {
            _connection = connection;
        }

        public IModel CreateChannel(string queueName)
        {
            _channel = _connection.CreateModel();

            Dictionary<string, object> args = new Dictionary<string, object>
            {
                { "x-max-priority", 3 }
            };

            _channel.QueueDeclare(queue: queueName,
                               durable: true,
                               exclusive: false,
                               autoDelete: false,
                               arguments: args);

            return _channel;
        }
    }
}