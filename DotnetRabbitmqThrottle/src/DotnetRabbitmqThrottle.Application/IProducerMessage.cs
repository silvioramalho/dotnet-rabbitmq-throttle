using System.Collections.Generic;
using DotnetRabbitmqThrottle.Application.ViewModels;

namespace DotnetRabbitmqThrottle.Application
{
    public interface IProducerMessage
    {
        void Send(IEnumerable<MessageViewModel> messages, string channelId);
    }
}