using RabbitMQ.Client;

namespace DotnetRabbitmqThrottle.Application
{
    public interface ISubscriptionManager
    {
        void Clear();

        bool Subscribe(string channelIdentity, IModel value);

        IModel Unsubscribe(string channelIdentity);

        IModel GetChannel(string channelIdentity);

        bool IsEmpty();

        int Count();
    }
}