using DotnetRabbitmqThrottle.Application;
using DotnetRabbitmqThrottle.Application.Services;
using DotnetRabbitmqThrottle.Application.ViewModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DotnetRabbitmqThrottle.Producer.ConsoleApp
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly WorkerParams _workerParams;
        private readonly IProducerMessage _producerMessage;
        private readonly BufferService _bufferChannel;

        public Worker(ILogger<Worker> logger,
            WorkerParams workerParams,
            IProducerMessage producerMessage,
            BufferService bufferChannel)
        {
            logger.LogInformation(
                $"Queue = {workerParams.QueueName}");

            _logger = logger;
            _producerMessage = producerMessage;
            _workerParams = workerParams;
            _bufferChannel = bufferChannel;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {          

            _logger.LogInformation(
                "Publishing messages...");

            //while (!stoppingToken.IsCancellationRequested)
            //{

                var stopwatch = new Stopwatch();

                stopwatch.Start();

                List<MessageViewModel> messages = new List<MessageViewModel>();

                for (int i = 0; i < _workerParams.TotalMessages; i++)
                {
                    var message = new MessageViewModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Content = "Message generated automaticaly",
                        Priority = 0,
                        To = "destination@queue.com",
                        From = _workerParams.QueueName
                    };
                    messages.Add(message);
                    
                    // Abordagem  Sugerida
                    await _bufferChannel.Writer.WriteAsync(message, CancellationToken.None);

                    //Abordagem Atual
                    //_producerMessage.Send(new List<MessageViewModel>() { message }, 1.ToString());
                }
                
                // Abordagem Perfeita (Mas impossível para o nosso cenário)
                //_producerMessage.Send(messages, 1.ToString());
                await Task.Delay(1000, stoppingToken);

                stopwatch.Stop();
                long media = _workerParams.TotalMessages / (stopwatch.ElapsedMilliseconds / 1000);
                _logger.LogInformation(
                    $"{_workerParams.TotalMessages} messages published in {stopwatch.ElapsedMilliseconds} ms(TPS: {media}/s)");

                await Task.Delay(1000, stoppingToken);
            //}
        }
    }
}

