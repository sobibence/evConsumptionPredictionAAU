using EVCP.DataConsumer.Dtos;

namespace EVCP.DataConsumer.Publisher;

public class PublishWorker : IWorker
{
    private readonly IEVDataPublisher _publisher;
    private readonly string _routingKey;
    private readonly int _noOfMessages;
    private readonly int _noOfElementsInMessage;

    public PublishWorker(IEVDataPublisher publisher, string routingKey, int noOfMessages = 10, int noOfElementsInMessage = 60)
    {
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        _routingKey = routingKey;
        _noOfMessages = noOfMessages;
        _noOfElementsInMessage = noOfElementsInMessage;
    }

    public async Task Run()
    {
        await ProcessFile();
    }

    private async Task ProcessFile()
    {
        var messages = GenerateMessages();

        messages.ToList().ForEach(async data => await _publisher.Publish(data, _routingKey));
    }

    private IEnumerable<IEVItemDto> GenerateMessage()
        => Enumerable.Range(0, _noOfElementsInMessage).Select(i => new EVItemDto($"test {i + 1}")).ToArray();

    private IEnumerable<IEVDataDto<IEVItemDto>> GenerateMessages()
        => Enumerable.Range(0, _noOfMessages).Select(_ => new EVDataDto<IEVItemDto>(DateTime.Now, GenerateMessage())).ToArray();
}
