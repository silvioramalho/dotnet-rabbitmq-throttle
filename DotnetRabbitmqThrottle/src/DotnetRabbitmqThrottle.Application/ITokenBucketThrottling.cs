namespace DotnetRabbitmqThrottle.Application
{
    public interface ITokenBucketThrottling
    {
        void ThrottlerSemaphore(string consumerKey, int requestsPerSecondThreshold);
    }
}