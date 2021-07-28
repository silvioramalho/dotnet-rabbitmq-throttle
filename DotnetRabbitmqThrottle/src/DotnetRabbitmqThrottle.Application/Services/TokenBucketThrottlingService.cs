using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace DotnetRabbitmqThrottle.Application.Services
{
    public class TokenBucketThrottlingService : ITokenBucketThrottling
    {
        private readonly IConnectionMultiplexer _connection;
        private const string QUEUE_PREFIX = "consumer.throttle";

        public TokenBucketThrottlingService(
            IConnectionMultiplexer connection)
        {
            _connection = connection;
        }

        public void ThrottlerSemaphore(string consumerKey, int requestsPerSecondThreshold)
        {
            var cache = _connection.GetDatabase();

            var consumerThrottleCacheKey = $"{QUEUE_PREFIX}#{consumerKey}";
            bool isMaxCapacityExceeded = true;

            while (isMaxCapacityExceeded)
            {
                var cacheResult = cache.HashIncrement(consumerThrottleCacheKey, 1);

                if (cacheResult == 1)
                {
                    cache.KeyExpire(consumerThrottleCacheKey,
                        TimeSpan.FromMilliseconds(1150),
                        CommandFlags.FireAndForget);
                }
                else if (cacheResult > requestsPerSecondThreshold)
                {
                    // Console.WriteLine("429 - Throughtput Max Capacity Exceeded");
                    Task.Delay(10).Wait();
                }

                isMaxCapacityExceeded = (cacheResult > requestsPerSecondThreshold);
            }
        }
    }
}