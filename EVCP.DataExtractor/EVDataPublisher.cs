using EasyNetQ;

namespace EVCP.DataExtractor
{
    public class EVDataPublisher : IEVDataPublisher
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

    public interface IEVDataPublisher
    {
        public Task Publish(EVData message);
    }
}
