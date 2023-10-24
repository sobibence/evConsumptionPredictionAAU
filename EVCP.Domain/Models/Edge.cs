using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

public class Edge : BaseEntity
{
    [ColumnName("length_meters")]
    public float LengthMeters { get; set; }

    [ColumnName("allowed_speed_kmph")]
    public int AllowedSpeedKmph { get; set; }

    [ColumnName("inclination_degress")]
    public float InclinationDegrees { get; set; }

    [ColumnName("start_node_id")]
    public int StartNodeId { get; set; }

    [ColumnName("end_node_id")]
    public int EndNodeId { get; set; }

    [ColumnName("average_speed_kmph")]
    public float AverageSpeedKmph { get; set; }

    [ColumnName("osm_way_id")]
    public long OsmWayId { get; set; }
}
