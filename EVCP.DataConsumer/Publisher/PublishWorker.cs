using EVCP.Dtos;

namespace EVCP.DataConsumer.Publisher;

public class PublishWorker : IWorker
{
    private readonly ITripDataPublisher _publisher;
    private readonly string _routingKey;
    private readonly Func<IEnumerable<ITripDataDto>> _getMessages;

    public PublishWorker(ITripDataPublisher publisher, string routingKey, Func<IEnumerable<ITripDataDto>> getMessages)
    {
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        _routingKey = routingKey;
        _getMessages = getMessages;
    }

    public async Task Run()
    {
        await ProcessFile();
    }

    private async Task ProcessFile()
    {
        var messages = _getMessages();

        messages.ToList().ForEach(async data => await _publisher.Publish(data, _routingKey));
    }
}
