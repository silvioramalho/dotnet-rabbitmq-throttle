using DotnetRabbitmqThrottle.Application;
using DotnetRabbitmqThrottle.Application.Services;
using DotnetRabbitmqThrottle.Application.ViewModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DotnetRabbitmqThrottle.Producer.ConsoleApp
{
    public class ProducerWorker : BackgroundService
    {
        private readonly ILogger<ProducerWorker> _logger;
        private readonly WorkerParams _workerParams;
        private readonly IProducerMessage _producerMessage;
        private readonly ISubscriptionManager _subscriptionManager;
        private readonly BufferService _bufferChannel;

        public ProducerWorker(ILogger<ProducerWorker> logger,
            WorkerParams workerParams,
            IProducerMessage producerMessage,
            ISubscriptionManager subscriptionManager,
            BufferService bufferChannel)
        {
            logger.LogInformation(
                $"Queue = {workerParams.QueueName}");

            _logger = logger;
            _producerMessage = producerMessage;
            _workerParams = workerParams;
            _bufferChannel = bufferChannel;
            _subscriptionManager = subscriptionManager;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            
            base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"Total processors: {Environment.ProcessorCount}");
            while (!stoppingToken.IsCancellationRequested)
            {
                var workers = new List<Task>();
                for (int i = 0; i < Environment.ProcessorCount; i++)
                { 
                    workers.Add(RunAsync(i, stoppingToken));
                }

                await Task.WhenAll(workers.ToArray());
            }

        }

        private async Task RunAsync(int number, CancellationToken stoppingToken)
        {          

            _logger.LogInformation(
                "Reading Channel...");

            while (!stoppingToken.IsCancellationRequested)
            {
                while (await _bufferChannel.Reader.WaitToReadAsync())
                {
                    if (_bufferChannel.Reader.TryRead(out var message))
                    {
                        Console.WriteLine($"[Worker({number})]: {message.Content}");
                        _producerMessage.Send(
                            new List<MessageViewModel>() { message },
                            number.ToString()
                        );
                    }
                }
                //Console.WriteLine("Saiu do While");

                //await Task.Delay(1, stoppingToken);
            }
            
        } 
    }
}

