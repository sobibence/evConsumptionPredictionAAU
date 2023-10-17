namespace EVCP.Domain.Models;

public class Node : BaseEntity
{
    public float latitude { get; set; }

    public float longitude { get; set; }

    public int longitude_meters { get; set; }

    public int osm_node_id { get; set; }
}
