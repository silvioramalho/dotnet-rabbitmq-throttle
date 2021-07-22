using System.Threading;
using System.Threading.Tasks;
using RestEase;

namespace DotnetRabbitmqThrottle.Consumer.ConsoletHost
{
    public interface IMessageSender
    {
        [Post("/")]
        Task SendMessageAsync(
            [Body] object request,
            CancellationToken cancellationToken);
    }
}
