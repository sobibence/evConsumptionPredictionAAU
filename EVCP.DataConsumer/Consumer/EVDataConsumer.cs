﻿using EasyNetQ;
using EasyNetQ.Topology;
using EVCP.Dtos;

namespace EVCP.DataConsumer.Consumer;

public interface IEVDataConsumer
{
    public Task Subscribe(Action<ITripDataDto> handler);
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

    public async Task Subscribe(Action<ITripDataDto> handler)
    {
        _bus.Consume<ITripDataDto>(_queue, async (msg, info) =>
        {
            _consumedMessagesCount++;

            // handle message
            await Task.Run(() =>
            {
                handler.Invoke(msg.Body);

                //Console.WriteLine($"Queue: {_queue.Name}\n" +
                //                  $"Consumed Messages Count: {_consumedMessagesCount}");
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
