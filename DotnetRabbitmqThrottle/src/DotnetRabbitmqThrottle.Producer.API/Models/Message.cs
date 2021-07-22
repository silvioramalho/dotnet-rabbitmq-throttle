using System.ComponentModel.DataAnnotations;

namespace DotnetRabbitmqThrottle.Producer.API.Models
{
    public class Message
    {
        [Required]
        public string Content { get; set; }

        public int? Priority { get; set; }
    }
}
