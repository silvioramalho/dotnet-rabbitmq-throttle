using DotnetRabbitmqThrottle.Producer.ConsoleApp;
using DotnetRabbitmqThrottle.Producer.ConsoleApp.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        string queueName = args.Length > 0 ? args[0] : hostContext.Configuration["DefaultQueueName"];
        var workerParams = new WorkerParams()
        {
            RabbitMQConnectionString = hostContext.Configuration.GetConnectionString("RabbitMQ"),
            QueueName = queueName,
            TotalMessages = int.Parse(hostContext.Configuration["TotalMessages"])
        };
        services.AddSingleton<WorkerParams>(workerParams);
        services.AddApplicationServices(workerParams);
        services.AddHostedService<ProducerWorker>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();