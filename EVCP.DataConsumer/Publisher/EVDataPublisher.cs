using EasyNetQ;
using EasyNetQ.Topology;
using EVCP.DataConsumer.Dtos;
using EVCP.Dtos;

namespace EVCP.DataConsumer.Publisher;

public interface IEVDataPublisher
{
    public Exchange Exchange { get; set; }

    public Task Publish<T>(IEVDataDto<T> data, string routingKey);

    public Task Publish(ITripDataDto data, string routingKey);
}

public class EVDataPublisher : IEVDataPublisher
{
    public Exchange Exchange { get; set; }

    private readonly IAdvancedBus _bus;

    public EVDataPublisher(IBus bus, string exchangeName)
    {
        _bus = bus.Advanced ?? throw new ArgumentNullException(nameof(bus));
        Exchange = CreateExchange(exchangeName);
    }

    public async Task Publish<T>(IEVDataDto<T> data, string routingKey)
    {
        var message = new Message<IEVDataDto<T>>(data);
        await _bus.PublishAsync(Exchange, routingKey, false, message);
    }

    public async Task Publish(ITripDataDto data, string routingKey)
    {
        var message = new Message<ITripDataDto>(data);
        await _bus.PublishAsync(Exchange, routingKey, false, message);
    }

    private Exchange CreateExchange(string name)
    {
        return _bus.ExchangeDeclare(name, ExchangeType.Direct);
    }
}
