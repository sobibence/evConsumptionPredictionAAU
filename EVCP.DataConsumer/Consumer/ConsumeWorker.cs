using EVCP.DataConsumer.Dtos;

namespace EVCP.DataConsumer.Consumer;

public class ConsumeWorker : IWorker
{
    private readonly IEVDataConsumer _consumer;

    public ConsumeWorker(IEVDataConsumer consumer)
    {
        _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
    }

    public async Task Run()
    {
        await ProcessFile();
    }

    private async Task ProcessFile()
    {
        await _consumer.Subscribe<IEVDataDto<IEVItemDto>>(MessageHandler);
    }

    void MessageHandler(IEVDataDto<IEVItemDto> message)
    {
        Console.WriteLine(message.SourceTimestamp);
        message.Data.ToList().ForEach(m => Console.WriteLine($"Message: {m.Name}"));
        Console.WriteLine(new string('-', 20));
    }
}
