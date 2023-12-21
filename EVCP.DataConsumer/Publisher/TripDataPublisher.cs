using EasyNetQ;
using EVCP.Dtos;

namespace EVCP.DataConsumer.Publisher;

public interface ITripDataPublisher
{
    public Task Publish(ITripDataDto data, string routingKey);
}

public class TripDataPublisher : ITripDataPublisher
{
    private readonly IBus _bus;

    public TripDataPublisher(IBus bus)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    }

    public async Task Publish(ITripDataDto data, string routingKey)
    {
        await _bus.PubSub.PublishAsync(data, routingKey);
    }
}
