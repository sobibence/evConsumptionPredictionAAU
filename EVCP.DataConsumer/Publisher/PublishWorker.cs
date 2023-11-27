using EVCP.DataConsumer.Dtos;

namespace EVCP.DataConsumer.Publisher;

public class PublishWorker : IWorker
{
    private readonly IEVDataPublisher _publisher;

    public static int NoOfMessages = 10;
    public static int NoOfElementsInMessage = 60;

    public PublishWorker(IEVDataPublisher publisher)
    {
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
    }

    public async Task Run()
    {
        await ProcessFile();
    }

    private async Task ProcessFile()
    {
        var messages = GenerateMessages();
        var publishTasks = messages.Select(_publisher.Publish);

        await Task.WhenAll(publishTasks);
    }

    private static IEnumerable<IEVItemDto> GenerateMessage()
        => Enumerable.Range(0, NoOfElementsInMessage).Select(i => new EVItemDto($"test {i}")).ToArray();

    private static IEnumerable<IEVDataDto<IEVItemDto>> GenerateMessages()
        => Enumerable.Range(0, NoOfMessages).Select(_ => new EVDataDto<IEVItemDto>(DateTime.Now, GenerateMessage())).ToArray();
}
