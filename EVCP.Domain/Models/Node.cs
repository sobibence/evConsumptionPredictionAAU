
namespace EVCP.Domain.Models;

using System.ComponentModel.DataAnnotations.Schema;
using EVCP.Domain.Helpers;
using NetTopologySuite.Geometries;

[TableName("node")]
public class Node : BaseEntity
{
    [ColumnName("osm_node_id")]
    public long NodeIdOsm { get; set; }


    private NetTopologySuite.Geometries.Point? _point;
    [ColumnName("gps_coords")]
    public NetTopologySuite.Geometries.Point Point
    {
        get
        {
            if (_point == null)
            {
                _point = new Point(0,0);   
            }
            return _point;
        }
        set
        {
            _point = value;
        }
    }

    [NotMapped]
    public double Latitude
    {
        get
        {
            return Point.Coordinate.Y;
        }
        set
        {
            Point.Coordinate.Y = value;
        }
    }

    [NotMapped]
    public double Longitude
    {
        get
        {
            return Point.Coordinate.X;
        }
        set
        {
            Point.Coordinate.X = value;
        }
    }

    [NotMapped]
    private List<Edge> _listOfConnectedEdges = new();
    [NotMapped]
    public List<Edge> ListOfConnectedEdges { get { return _listOfConnectedEdges; } }

    private List<Node> _listOfConnectedNodes = new();
    [NotMapped]
    public List<Node> ListOfConnectedNodes { get { return _listOfConnectedNodes; } }


    public override string ToString()
    {
        string connectedNodes = string.Join(", ", _listOfConnectedNodes.Select(node => node.NodeIdOsm));

        return $"Node Info:\n" +
               $"NodeIdOsm: {NodeIdOsm}\n" +
               $"Latitude: {Latitude}\n" +
               $"Longitude: {Longitude}\n" +
               $"Connected Nodes: [{connectedNodes}]";
    }
}
