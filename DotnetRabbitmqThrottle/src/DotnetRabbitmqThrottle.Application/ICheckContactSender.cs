using System.Threading;
using System.Threading.Tasks;
using RestEase;

namespace DotnetRabbitmqThrottle.Application
{
    public interface ICheckContactSender
    {
        [Post("/")]
        Task CheckContactAsync(
            [Body] object request,
            CancellationToken cancellationToken);
    }
}