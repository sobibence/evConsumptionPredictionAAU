namespace EVCP.Domain.Models;

public class Node : BaseEntity
{
    public float Latitude { get; set; }

    public float Longitude { get; set; }

    public int LongitudeMeters { get; set; }

    public int OsmNodeId { get; set; }
}
