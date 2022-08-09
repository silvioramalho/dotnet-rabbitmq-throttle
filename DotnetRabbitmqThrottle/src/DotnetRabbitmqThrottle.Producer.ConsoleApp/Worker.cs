﻿using DotnetRabbitmqThrottle.Application;
using DotnetRabbitmqThrottle.Application.ViewModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotnetRabbitmqThrottle.Producer.ConsoleApp
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly WorkerParams _workerParams;
        private readonly IProducerMessage _producerMessage;

        public Worker(ILogger<Worker> logger,
            WorkerParams workerParams,
            IProducerMessage producerMessage)
        {
            logger.LogInformation(
                $"Queue = {workerParams.QueueName}");

            _logger = logger;
            _producerMessage = producerMessage;
            _workerParams = workerParams;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}

            _logger.LogInformation(
                "Publishing messages...");

            List<MessageViewModel> messages = new List<MessageViewModel>();

            for (int i = 0; i < _workerParams.TotalMessages; i++)
            {
                messages.Add(new MessageViewModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    Content = "Message generated automaticaly",
                    Priority = 0,
                    To = "destination@queue.com",
                    From = _workerParams.QueueName
                });
            }

            _producerMessage.Send(messages);

            await Task.Delay(1, stoppingToken);
        }
    }
}

