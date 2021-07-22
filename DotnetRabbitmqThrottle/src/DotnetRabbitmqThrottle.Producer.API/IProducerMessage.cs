using System.Collections.Generic;
using DotnetRabbitmqThrottle.Producer.API.Models;

namespace DotnetRabbitmqThrottle.Producer.API
{
    public interface IProducerMessage
    {
        void Send(IEnumerable<Message> messages);
    }
}
