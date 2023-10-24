using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

public class Node : BaseEntity
{
    [ColumnName("latitude")]
    public float Latitude { get; set; }

    [ColumnName("longitude")]
    public float Longitude { get; set; }

    [ColumnName("longitude_meters")]
    public int LongitudeMeters { get; set; }

    [ColumnName("osm_node_id")]
    public long OsmNodeId { get; set; }
}
