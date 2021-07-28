namespace DotnetRabbitmqThrottle.Application
{
    public interface IConsumerMessage
    {
        void Consume(string queueName);
    }
}