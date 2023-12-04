using EVCP.DataConsumer.Dtos;

namespace EVCP.DataConsumer.Publisher;

public class MessageGenerator
{
    private readonly int _noOfMessages;
    private readonly int _noOfElementsInMessage;

    public MessageGenerator(int noOfMessages = 10, int noOfElementsInMessage = 60)
    {
        _noOfMessages = noOfMessages;
        _noOfElementsInMessage = noOfElementsInMessage;
    }
    public IEnumerable<IEVDataDto<IEVItemDto>> GenerateMessage()
        => Enumerable.Range(0, _noOfMessages).Select(_ => new EVDataDto<IEVItemDto>(DateTime.Now, GenerateMessageItem())).ToArray();

    private IEnumerable<IEVItemDto> GenerateMessageItem()
        => Enumerable.Range(0, _noOfElementsInMessage).Select(i => new EVItemDto($"test {i + 1}")).ToArray();

    //public IEnumerable<IEVDataDto<IEVItemDto>> GenerateTripMessage()
    //    => Enumerable.Range(0, _noOfMessages).Select(_ => new EVDataDto<IEVItemDto>(DateTime.Now, GenerateMessageItem())).ToArray();

    //private IEnumerable<IEVItemDto> GenerateTripMessageItem()
    //    => Enumerable.Range(0, _noOfElementsInMessage).Select(i => new EVItemDto($"test {i + 1}")).ToArray();
}
