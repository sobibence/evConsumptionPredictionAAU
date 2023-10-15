using EVCP.DataAccess.Repositories;

namespace EVCP.Domain.Models;

public class Edge : BaseEntity
{
    public float length_meters { get; set; }

    public int allowed_speed_kmph { get; set; }

    public int inclination_degrees { get; set; }

    public int start_node_id { get; set; }

    public int end_node_id { get; set; }

    public float average_speed_kmph { get; set; }

    public int osm_way_id { get; set; }
}
