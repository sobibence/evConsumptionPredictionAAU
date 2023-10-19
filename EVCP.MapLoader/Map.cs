using EVCP.Domain.Models;

namespace EVCP.MapLoader;
public struct Map
{

    public List<Node> Nodes { get; set; }
    public List<Edge> Edges { get; set; }
}
