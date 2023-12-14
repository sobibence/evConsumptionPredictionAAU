﻿using EasyNetQ;
using EVCP.Dtos;

namespace EVCP.DataConsumer.Consumer;

public interface IEVDataConsumer
{
    public Task Subscribe(Action<ITripDataDto> handler);
}

public class TripDataConsumer : IEVDataConsumer
{
    private readonly IBus _bus;
    private readonly string _routingKey;
    private readonly string _subscriptionId;

    public TripDataConsumer(IBus bus, string subscriptionId, string routingKey)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _routingKey = routingKey;
        _subscriptionId = subscriptionId;
    }

    public async Task Subscribe(Action<ITripDataDto> handler)
    {
        await _bus.PubSub.SubscribeAsync<ITripDataDto>(_subscriptionId, handler.Invoke, x => x.WithTopic(_routingKey));
    }
}
