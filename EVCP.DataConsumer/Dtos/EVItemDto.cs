namespace EVCP.DataConsumer.Dtos;

public interface IEVItemDto
{
    public string Name { get; }
}

public class EVItemDto : IEVItemDto
{
    public string Name { get; }

    public EVItemDto(string name)
    {
        Name = name;
    }
}
