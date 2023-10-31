using EasyNetQ;

namespace EVCP.DataExtractor
{
    public class EVDataPublisher : IMessagePublisher
    {
        private readonly IBus _bus;

        public EVDataPublisher(IBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public async Task Publish(EVData message)
        {
            await _bus.PubSub.PublishAsync((IEVData)message);
        }
    }

    public interface IMessagePublisher
    {
        public Task Publish(EVData message);
    }
}
