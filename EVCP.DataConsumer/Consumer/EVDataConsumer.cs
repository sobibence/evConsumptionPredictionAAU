using EasyNetQ;
using EasyNetQ.Topology;
using EVCP.DataConsumer.Dtos;

namespace EVCP.DataConsumer.Consumer;

public interface IEVDataConsumer
{
    public Task Subscribe<T>(Action<IEVDataDto<T>> handler);
}

public class EVDataConsumer : IEVDataConsumer
{
    private readonly IAdvancedBus _bus;
    private readonly Queue _queue;

    private int _consumedMessagesCount = 0;

    public EVDataConsumer(IBus bus, string queueName, Exchange exchange, string routingKey)
    {
        _bus = bus.Advanced ?? throw new ArgumentNullException(nameof(bus));
        _queue = CreateQueue(queueName);
        BindQueues(exchange, routingKey);
    }

    public async Task Subscribe<T>(Action<IEVDataDto<T>> handler)
    {
        _bus.Consume<IEVDataDto<T>>(_queue, async (msg, info) =>
        {
            await Task.Run(() =>
            {
                //handler.Invoke(msg.Body);
                _consumedMessagesCount++;
                Console.WriteLine($"Queue: {_queue.Name}\n" +
                                  $"Consumed Messages Count: {_consumedMessagesCount}");
            });
        });
    }

    private Queue CreateQueue(string name)
    {
        return _bus.QueueDeclare(name);
    }

    private void BindQueues(Exchange exchange, string routingKey)
    {
        _bus.Bind(exchange, _queue, routingKey);
    }
}
