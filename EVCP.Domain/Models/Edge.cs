namespace EVCP.Domain.Models;

public class Edge : BaseEntity
{
    public float LengthMeters { get; set; }

    public int AllowedSpeedKmph { get; set; }

    public float InclinationDegrees { get; set; }

    public int StartNodeId { get; set; }

    public int EndNodeId { get; set; }

    public float AverageSpeedKmph { get; set; }

    public long OsmWayId { get; set; }
}
