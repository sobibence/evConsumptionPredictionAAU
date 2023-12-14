using EasyNetQ;
using EasyNetQ.Topology;
using EVCP.Dtos;

namespace EVCP.DataConsumer.Publisher;

public interface ITripDataPublisher
{
    public Exchange Exchange { get; set; }

    public Task Publish(ITripDataDto data, string routingKey);
}

public class TripDataPublisher : ITripDataPublisher
{
    public Exchange Exchange { get; set; }

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
