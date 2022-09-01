using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace DotnetRabbitmqThrottle.Application.Services
{
    public class SubscriptionManager : ISubscriptionManager
    {
        private readonly ConcurrentDictionary<string, IModel> _subscriptions;
        private readonly ILogger<SubscriptionManager> _logger;

        public SubscriptionManager(
            ILogger<SubscriptionManager> logger)
        {
            _subscriptions = new ConcurrentDictionary<string, IModel>();
            _logger = logger;
        }

        public bool Subscribe(string channelIdentity, IModel value)
        {
            if (_subscriptions.TryAdd(channelIdentity, value))
            {
                return true;
            }

            _logger.LogError(
                "[{Source}] Error to add channel for queue: {queueName}.",
                nameof(SubscriptionManager),
                channelIdentity);

            return false;
        }

        public IModel Unsubscribe(string channelIdentity)
        {
            if (string.IsNullOrWhiteSpace(channelIdentity))
            {
                throw new ArgumentNullException(nameof(channelIdentity));
            }

            if (!_subscriptions.TryRemove(channelIdentity, out var channel))
            {
                _logger.LogError(
                    "[{Source}] Error to remove channel with queue name {queueName} from subscription manager.",
                    nameof(SubscriptionManager),
                    channelIdentity);
            }

            return channel;
        }

        public IModel GetChannel(string channelIdentity)
        {
            if (string.IsNullOrWhiteSpace(channelIdentity))
            {
                throw new ArgumentNullException(nameof(channelIdentity));
            }

            if (_subscriptions.TryGetValue(channelIdentity, out var channel))
            {
                return channel;
            }
            else
            {
                _logger.LogWarning(
                    "[{Source}] Subscription not found in subscription manager for queue {queueName}.",
                    nameof(SubscriptionManager),
                    channelIdentity);

                return null;
            }
        }

        public void Clear() => _subscriptions.Clear();

        public bool IsEmpty() => _subscriptions.IsEmpty;

        public int Count() => _subscriptions.Count;
    }


}
