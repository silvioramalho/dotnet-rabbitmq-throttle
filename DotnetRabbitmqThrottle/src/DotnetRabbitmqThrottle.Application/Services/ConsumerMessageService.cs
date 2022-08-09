using System;
using System.Text;
using System.Threading;
using DotnetRabbitmqThrottle.Application.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RestEase;

namespace DotnetRabbitmqThrottle.Application.Services
{
    public class ConsumerMessageService : IConsumerMessage
    {
        private readonly ILogger<ConsumerMessageService> _logger;
        private readonly ITokenBucketThrottling _tokenBucketThrottling;
        private readonly IModel _channel;
        private readonly ushort _throughput;
        private readonly string _containerCheckContactUrl;
        private string _queueName;

        public ConsumerMessageService(
            ITokenBucketThrottling tokenBucketThrottling,
            IConfiguration configuration,
            IModel channel,
            ILogger<ConsumerMessageService> logger)
        {
            _tokenBucketThrottling = tokenBucketThrottling;
            _logger = logger;
            _channel = channel;
            _throughput = Convert.ToUInt16(configuration["RequestsPerSecondThreshold"]);
            _containerCheckContactUrl = configuration["ContainerCheckContactUrl"];
        }

        public void Consume(
            string queueName
            )
        {
            _queueName = queueName;
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += Consumer_Received;

            _channel.BasicQos(0, _throughput, false);
            _channel.BasicConsume(queue: _queueName,
                autoAck: false,
                consumer: consumer);
        }

        private void Consumer_Received(
            object sender, BasicDeliverEventArgs ea)
        {
            //_tokenBucketThrottling.ThrottlerSemaphore(_queueName, Convert.ToInt32(_throughput));

            var messagecontent = Encoding.UTF8.GetString(ea.Body.ToArray()).DeserializeMessage();
            _logger.LogInformation(
                $"[New message | {DateTime.Now:yyyy-MM-dd HH:mm:ss}]" + messagecontent.Content);

            try
            {
                //CheckContactMessage(new { blocking = "wait", contacts = new[] { "+5531999991111" } });
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception e)
            {
                _channel.BasicNack(deliveryTag: ea.DeliveryTag, false, true);
                Console.WriteLine($"Exception: ${e.Message}");
            }
        }

        private void CheckContactMessage(object content)
        {
            var sender = RestClient.For<ICheckContactSender>(_containerCheckContactUrl);
            sender.CheckContactAsync(content, CancellationToken.None).Wait();
        }
    }
}