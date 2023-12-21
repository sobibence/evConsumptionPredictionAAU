namespace EVCP.DataConsumer.Dtos;

public interface IEVDataDto<T>
{
    public DateTime SourceTimestamp { get; set; }

    public IEnumerable<T> Data { get; set; }
}

public class EVDataDto<T> : IEVDataDto<T>
{
    public DateTime SourceTimestamp { get; set; }
    public IEnumerable<T> Data { get; set; }

    public EVDataDto(DateTime sourceTimestamp, IEnumerable<T> data)
    {
        SourceTimestamp = sourceTimestamp;
        Data = data;
    }
}
