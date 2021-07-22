using System;
using System.Collections.Generic;
using System.Text;
using DotnetRabbitmqThrottle.Producer.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace DotnetRabbitmqThrottle.Producer.API
{
    public class ProducerMessage : IProducerMessage
    {
        private readonly ILogger<ProducerMessage> _logger;
        private readonly IConfiguration _configuration;

        public ProducerMessage(
            IConfiguration configuration,
            ILogger<ProducerMessage> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void Send(IEnumerable<Message> messages)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(_configuration.GetConnectionString("RabbitMQ"))
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                string queueName = _configuration["QueueName"];
                Dictionary<string, object> args = new Dictionary<string, object>
                {
                    { "x-max-priority", 3 }
                };

                channel.QueueDeclare(queue: queueName,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: args);

                foreach (var message in messages)
                {
                    IBasicProperties props = channel.CreateBasicProperties();
                    props.Priority = (byte)(message.Priority ?? 0);

                    channel.BasicPublish(exchange: "",
                                         routingKey: queueName,
                                         basicProperties: props,
                                         body: Encoding.UTF8.GetBytes(message.Content));
                    _logger.LogInformation(
                        $"[Message sended] {message}");
                }

                _logger.LogInformation("Concluido o envio de mensagens");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.GetType().FullName} | " +
                             $"Message: {ex.Message}");
            }
        }
    }
}