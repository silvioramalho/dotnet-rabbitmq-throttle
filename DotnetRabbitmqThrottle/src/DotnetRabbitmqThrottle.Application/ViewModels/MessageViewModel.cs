using System.ComponentModel.DataAnnotations;

namespace DotnetRabbitmqThrottle.Application.ViewModels
{
    public class MessageViewModel
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public int? Priority { get; set; }

        public string To { get; set; }

        public string From { get; set; }
    }
}
