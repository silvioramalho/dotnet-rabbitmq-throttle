using DotnetRabbitmqThrottle.Application.ViewModels;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DotnetRabbitmqThrottle.Application.Services
{
    public class BufferService
    {
        private readonly Channel<MessageViewModel> _channel;

        public BufferService()
        {
            _channel = Channel.CreateUnbounded<MessageViewModel>(
                new UnboundedChannelOptions() { 
                    SingleReader = true,
                }
            );
        }

        public ChannelWriter<MessageViewModel> Writer => _channel.Writer;

        public ChannelReader<MessageViewModel> Reader => _channel.Reader;

        public Task<MessageViewModel> ReceiveAsync(CancellationToken cancellationToken) => _channel.Reader.ReadAsync(cancellationToken).AsTask();

        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public void Stop()
        {
            _channel.Writer.TryComplete();
        }

        public Task Execution => Task.CompletedTask;
    }
}
