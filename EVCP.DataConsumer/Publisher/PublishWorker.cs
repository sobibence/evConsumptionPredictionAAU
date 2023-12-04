using EVCP.Dtos;

namespace EVCP.DataConsumer.Publisher;

public class PublishWorker : IWorker
{
    private readonly IEVDataPublisher _publisher;
    private readonly string _routingKey;

    private readonly Func<IEnumerable<ITripDataDto>> _generateMessages;

    public PublishWorker(IEVDataPublisher publisher, string routingKey, Func<IEnumerable<ITripDataDto>> generateMessages)
    {
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        _routingKey = routingKey;
        _generateMessages = generateMessages;
    }

    public async Task Run()
    {
        await ProcessFile();
    }

    private async Task ProcessFile()
    {
        var messages = _generateMessages();

        messages.ToList().ForEach(async data => await _publisher.Publish(data, _routingKey));
    }
}
