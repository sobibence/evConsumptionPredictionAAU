namespace EVCP.DataConsumer.Consumer;

public class ConsumeWorker : IWorker
{
    private readonly IEVDataConsumer _consumer;
    private ITripDataHandler _handler;
    private readonly string name;

    public ConsumeWorker(IEVDataConsumer consumer, ITripDataHandler handler, string name)
    {
        _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        this.name = name ?? "n/a";
    }

    public async Task Run()
    {
        await ProcessFile();
    }

    private async Task ProcessFile()
    {
        await _consumer.Subscribe(_handler.Handle);
    }
}
