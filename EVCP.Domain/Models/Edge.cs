using System.ComponentModel.DataAnnotations.Schema;
using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

[TableName("edge")]
public class Edge : BaseEntity
{
    [ColumnName("start_node_id")]
    public long StartNodeId { get; set; }
    [ColumnName("end_node_id")]
    public long EndNodeId { get; set; }

    
    private double _length;

    public double Length
    {
        get
        {
            if (_length == 0)
            {
                _length = GpsDistanceCalculator.CalculateDistance(StartNode, EndNode);
            }
            return _length;
        }
        set
        {
            _length = value;
        }
    } // this should be in meters

    private Node _startNode;
    [NotMapped]
    public Node StartNode
    {
        get
        {
            if (_startNode == null)
            {
                //TODO: attempt to get it from db if the id exist
                throw new NullReferenceException();
            }
            return _startNode;
        }

        set
        {
            if (StartNodeId == 0)
            {
                StartNodeId = value.NodeIdOsm;
            }

            _startNode = value;
        }
    }

    private Node _endNode;
    [NotMapped]
    public Node EndNode
    {
        get
        {
            if (_endNode == null)
            {
                //TODO: attempt to get it from db if the id exist
                throw new NullReferenceException();
            }
            return _endNode;
        }
        set
        {
            if (EndNodeId == 0)
            {
                EndNodeId = value.NodeIdOsm;
            }
            _endNode = value;

        }
    }

    public int EdgeInfoId{get;set;}

    private EdgeInfo _edgeInfo;
    [NotMapped]
    public EdgeInfo EdgeInfo
    {
        get { return _edgeInfo;}
        set
        {
            _edgeInfo = value;
        }
    }
    public override string ToString()
    {
        return $"Way Info:\n" +
               $"StartNodeId: {StartNodeId}\n" + 
               $"EndNodeId: {EndNodeId}\n" +
               $"Speed Limit: {EdgeInfo.SpeedLimit}\n" +
               $"Street Name: {EdgeInfo.StreetName}\n" +
               $"Highway: {EdgeInfo.Highway}\n" +
               $"Surface: {EdgeInfo.Surface}";
    }
}

public class GpsDistanceCalculator
{
    //shamelessly stolen from chatgpt
    public static double CalculateDistance(Node point1, Node point2)
    {
        double earthRadius = 6371000; // Earth's radius in meters 

        double lat1Rad = Math.PI * point1.Latitude / 180;
        double lon1Rad = Math.PI * point1.Longitude / 180;
        double lat2Rad = Math.PI * point2.Latitude / 180;
        double lon2Rad = Math.PI * point2.Longitude / 180;

        double dLat = lat2Rad - lat1Rad;
        double dLon = lon2Rad - lon1Rad;

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        double distance = earthRadius * c;

        return distance;
    }
}

