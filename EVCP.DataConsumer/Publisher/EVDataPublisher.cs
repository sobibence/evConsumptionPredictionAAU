using EasyNetQ;
using EVCP.DataConsumer.Dtos;

namespace EVCP.DataConsumer.Publisher;

public interface IEVDataPublisher
{
    public Task Publish<T>(IEVDataDto<T> message);
}

public class EVDataPublisher : IEVDataPublisher
{
    private readonly IBus _bus;

    public EVDataPublisher(IBus bus)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    }

    public async Task Publish<T>(IEVDataDto<T> message)
    {
        await _bus.PubSub.PublishAsync(message);
    }
}
