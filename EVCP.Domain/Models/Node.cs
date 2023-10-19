namespace EVCP.Domain.Models;

public class Node : BaseEntity
{
    public long NodeIdOsm { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    private List<Edge> _listOfConnectedEdges = new();
    public List<Edge> ListOfConnectedEdges { get { return _listOfConnectedEdges; } }

    public override string ToString()
    {
        string connectedEdges = string.Join(", ", _listOfConnectedEdges.Select(edge => edge.OsmWayId));

        return $"Node Info:\n" +
               $"NodeIdOsm: {NodeIdOsm}\n" +
               $"Latitude: {Latitude}\n" +
               $"Longitude: {Longitude}\n" +
               $"Connected Edges: [{connectedEdges}]";
    }
}
