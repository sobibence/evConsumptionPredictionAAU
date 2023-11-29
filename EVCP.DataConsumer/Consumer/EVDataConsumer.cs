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
            //msg.Body.Data.ToList().ForEach(ele => handler.BeginInvoke(ele, null, null));

            await Task.Run(() =>
            {
                handler.Invoke(msg.Body);
                Console.WriteLine(new string('-', 30));
                Console.WriteLine($"Queue: {_queue.Name}");
                Console.WriteLine($"Consumed Messages Count: {++_consumedMessagesCount}");
                Console.WriteLine(new string('-', 30));
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

    public static QueueStats QueueInfo(IBus bus, string name)
    {
        return bus.Advanced.GetQueueStats(name);
    }
}
