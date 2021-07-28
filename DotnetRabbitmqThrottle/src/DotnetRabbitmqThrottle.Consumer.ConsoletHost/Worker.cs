using System;
using System.Threading;
using System.Threading.Tasks;
using DotnetRabbitmqThrottle.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotnetRabbitmqThrottle.Consumer.ConsoletHost
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConsumerMessage _consumerMessager;
        private readonly int _workerIntervalActive;
        private readonly WorkerParams _workerParams;

        public Worker(ILogger<Worker> logger,
            IConfiguration configuration,
            WorkerParams workerParams,
            IConsumerMessage consumerMessager)
        {
            logger.LogInformation(
                $"Queue = {workerParams.QueueName}");

            _logger = logger;
            _consumerMessager = consumerMessager;
            _workerIntervalActive =
                Convert.ToInt32(configuration["WorkerIntervalActive"]);
            _workerParams = workerParams;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Wainting messages...");

            _consumerMessager.Consume(
                _workerParams.QueueName
                );

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    $"Worker was activated in: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                await Task.Delay(_workerIntervalActive, stoppingToken);
            }
        }
    }
}