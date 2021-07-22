namespace DotnetRabbitmqThrottle.Consumer.ConsoletHost
{
    public class WorkerParams
    {
        public string ConnectionString { get; init; }
        public string Queue { get; init; }
    }
}