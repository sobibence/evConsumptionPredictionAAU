using EasyNetQ;

namespace EVCP.DataConsumer.Consumer;

public interface IEVDataConsumer
{
    public Task Subscribe<T>(Action<T> handler);
}

public class EVDataConsumer : IEVDataConsumer
{
    private readonly IBus _bus;

    public EVDataConsumer(IBus bus)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    }

    public async Task Subscribe<T>(Action<T> handler)
    {
        await _bus.PubSub.SubscribeAsync<T>("test", handler);
    }
}
