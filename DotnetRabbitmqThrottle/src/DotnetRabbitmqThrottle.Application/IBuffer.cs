using DotnetRabbitmqThrottle.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetRabbitmqThrottle.Application
{
    public interface IBuffer
    {
        Task<MessageViewModel> ReceiveAsync(CancellationToken cancellationToken);

        Task WriteAsync(MessageViewModel message, CancellationToken cancellationToken);
    }
}
