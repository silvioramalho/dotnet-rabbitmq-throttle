namespace DotnetRabbitmqThrottle.Domain.Models
{
    public class Message
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string To { get; set; }
    }
}