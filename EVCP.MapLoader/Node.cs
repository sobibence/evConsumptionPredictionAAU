namespace EVCP.MapLoader;

public class Node
{
    public long NodeIdOsm { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    private List<Edge> _listOfConnectedEdges = new();
    public List<Edge> ListOfConnectedEdges { get { return _listOfConnectedEdges; } }
}
