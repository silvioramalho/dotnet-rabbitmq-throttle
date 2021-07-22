using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RestEase;

namespace DotnetRabbitmqThrottle.Consumer.ConsoletHost
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly int _workerIntervalActive;
        private readonly string _containerMessageUrl;
        private readonly WorkerParams _workerParams;
        private DateTime _lastDateTime;
        private int _sendedObjects;

        public Worker(ILogger<Worker> logger,
            IConfiguration configuration,
            WorkerParams workerParams)
        {
            logger.LogInformation(
                $"Queue = {workerParams.Queue}");

            _logger = logger;
            _workerIntervalActive =
                Convert.ToInt32(configuration["WorkerIntervalActive"]);
            _containerMessageUrl = configuration["ContainerMessageUrl"];
            _workerParams = workerParams;
            _lastDateTime = DateTime.Now;
            _sendedObjects = 0;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Wainting messages...");

            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_workerParams.ConnectionString)
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            Dictionary<string, object> args = new Dictionary<string, object>
            {
                { "x-max-priority", 3 }
            };

            channel.QueueDeclare(queue: _workerParams.Queue,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: args);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, ea) =>
            {
                _sendedObjects++;
                HandleThroughtput();  

                var messagecontent = Encoding.UTF8.GetString(ea.Body.ToArray());
                _logger.LogInformation(
                    $"[New message | {DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                    messagecontent);
                // SendMessage(new { content = messagecontent});

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicQos(0, 10, false);
            channel.BasicConsume(queue: _workerParams.Queue,
                autoAck: false,
                consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    $"Worker was activated in: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                await Task.Delay(_workerIntervalActive, stoppingToken);
            }
        }

        private void HandleThroughtput()
        {
            DateTime currentDate = DateTime.Now;
            long elapsedTicks = currentDate.Ticks - _lastDateTime.Ticks;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

            if (elapsedSpan.TotalMilliseconds >= 1000)
            {
                _lastDateTime = DateTime.Now;
                _sendedObjects = 1;
            } 
            else
            {
                if (_sendedObjects > 10)
                {
                    _sendedObjects = 1;
                    var diff = (int)(1000 - elapsedSpan.TotalMilliseconds);
                    Task.Delay(diff).Wait();
                    _lastDateTime = DateTime.Now;
                }                
            }
        }

        private void SendMessage(object content)
        {
            var sender = RestClient.For<IMessageSender>(_containerMessageUrl);
            sender.SendMessageAsync(content, CancellationToken.None).Wait();
        }
    }
}
