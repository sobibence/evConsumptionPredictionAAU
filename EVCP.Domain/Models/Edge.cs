using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;
public class Edge : BaseEntity
{

    public long StartNodeId { get; set; }
    public long EndNodeId { get; set; }

    private Node _startNode;
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
    public long OsmWayId { get; set; }

    //if we already calcuted it once then we dont need to do it again
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
    public int SpeedLimit { get; set; }

    private string _streetName = "";
    public string StreetName
    {
        get
        {
            return _streetName;
        }
        set
        {
            if (value is not null)
            {
                _streetName = value;
            }
        }
    }

    private string _highway = "";
    public string Highway
    {
        get
        {
            return _highway;
        }
        set
        {
            if (value is not null)
            {
                _highway = value;
            }
        }
    }

    private string _surface = "";
    public string Surface
    {
        get
        {
            return _surface;
        }
        set
        {
            if (value is not null)
            {
                _surface = value;
            }
        }
    }

    public override string ToString()
    {
        return $"Way Info:\n" +
               $"OsmWayId: {OsmWayId}\n" +
               $"StartNode: {StartNodeId}\n" +
               $"EndNode: {EndNodeId}\n" +
               $"Length: {Length} meters\n" +
               $"Speed Limit: {SpeedLimit}\n" +
               $"Street Name: {StreetName}\n" +
               $"Highway: {Highway}\n" +
               $"Surface: {Surface}";
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

