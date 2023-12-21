using EVCP.Domain.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVCP.Domain.Models;

[TableName("edge")]
public class Edge : BaseEntity
{
    //[ColumnName("start_node_id")]
    //public long StartNodeId { get; set; }

    //[ColumnName("end_node_id")]
    //public long EndNodeId { get; set; }

    //[ColumnName("edge_info_id")]
    //public long EdgeInfoId { get; set; }

    [ColumnName("start_node_id")]
    public long StartNodeId { get; set; }
    [ColumnName("end_node_id")]
    public long EndNodeId { get; set; }


    [NotMapped]
    private double _length;
    [NotMapped]
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

    [NotMapped]
    private Node? _startNode;
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

    [NotMapped]
    private Node? _endNode;
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

    private long _edgeInfoId;
    [ColumnName("edge_info_id")]
    public long EdgeInfoId
    {
        get
        {
            if (_edgeInfoId == 0 && _edgeInfo is not null)
            {
                _edgeInfoId = EdgeInfo.OsmWayId;
            }
            return _edgeInfoId;
        }
        set
        {
            _edgeInfoId = value;

        }
    }

    private EdgeInfo? _edgeInfo;
    [NotMapped]
    public EdgeInfo EdgeInfo
    {
        get
        {
            if (_edgeInfo == null)
            {
                //TODO: attempt to get it from db if the id exist
                throw new NullReferenceException();
            }
            return _edgeInfo;
        }
        set
        {
            if (EdgeInfoId == 0)
            {
                EdgeInfoId = value.OsmWayId;
            }
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



