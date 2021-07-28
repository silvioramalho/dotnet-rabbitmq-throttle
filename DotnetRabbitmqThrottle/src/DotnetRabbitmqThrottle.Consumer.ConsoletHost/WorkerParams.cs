namespace DotnetRabbitmqThrottle.Consumer.ConsoletHost
{
    public class WorkerParams
    {
        public string RabbitMQConnectionString { get; init; }
        public string QueueName { get; init; }
        public string RedisConnectionString { get; init; }
    }
}