﻿using EVCP.DataConsumer.Dtos;

namespace EVCP.DataConsumer.Consumer;

public class ConsumeWorker : IWorker
{
    private readonly IEVDataConsumer _consumer;
    private readonly string name;

    public ConsumeWorker(IEVDataConsumer consumer, string name)
    {
        _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        this.name = name ?? "n/a";
    }

    public async Task Run()
    {
        await ProcessFile();
    }

    private async Task ProcessFile()
    {
        await _consumer.Subscribe<IEVItemDto>(MessageHandler);
    }

    void MessageHandler(IEVDataDto<IEVItemDto> message)
    {
        message.Data.ToList().ForEach(m => Console.WriteLine($"$Consumer: {name}\tMessage: {m.Name}"));
    }
}
