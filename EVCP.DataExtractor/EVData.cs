namespace EVCP.DataExtractor
{
    public record EVData(string Name) : IEVData;

    public interface IEVData
    {
           public string Name { get; }
    }
}
