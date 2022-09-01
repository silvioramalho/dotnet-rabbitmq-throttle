using System;
namespace DotnetRabbitmqThrottle.Producer.ConsoleApp
{
    public class WorkerParams
    {
        public string RabbitMQConnectionString { get; init; }
        public string QueueName { get; init; }
        public int TotalMessages { get; init; }
        public int WorkersNumber { get; init; }
    }
}

