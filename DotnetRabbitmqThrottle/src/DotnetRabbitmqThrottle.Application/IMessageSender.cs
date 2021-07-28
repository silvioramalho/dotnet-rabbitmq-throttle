using System.Threading;
using System.Threading.Tasks;
using RestEase;

namespace DotnetRabbitmqThrottle.Application
{
    public interface IMessageSender
    {
        [Post("/")]
        Task SendMessageAsync(
            [Body] object request,
            CancellationToken cancellationToken);
    }
}