using EVCP.MapLoader.GpsDistanceCalculator;

namespace EVCP.Domain.Models;
public class Edge : BaseEntity
{

    public long StartNodeId {get;set;}
    public long EndNodeId {get;set;}

    private Node _startNode;
    public Node StartNode {
        get{
            if(_startNode == null){
                //TODO: attempt to get it from db if the id exist
                throw new NullReferenceException();
            }
            return _startNode;
        }
    
        set
        {
            if(StartNodeId == 0){
                StartNodeId = value.NodeIdOsm;
            }

            _startNode = value;
        }
    }

    private Node _endNode;
    public Node EndNode {
        get{
            if(_endNode == null){
                //TODO: attempt to get it from db if the id exist
                throw new NullReferenceException();
            }
            return _endNode;
        }
        set
        {
            if(EndNodeId == 0){
                EndNodeId = value.NodeIdOsm;
            }
            _endNode = value;
            
        }
    }
    public long OsmWayId{get;set;}

    //if we already calcuted it once then we dont need to do it again
    private double _length;
    public double Length{
        get{
            if (_length == 0){
                _length = GpsDistanceCalculator.CalculateDistance(StartNode,EndNode);
            }
            return _length;
        }
        set{
            _length = value;
        }
    } // this should be in meters
    public int SpeedLimit{get;set;}

    private string _streetName = "";
    public string StreetName{
        get{
            return _streetName;
        }
        set{
            if(value is not null){
                _streetName = value;
            }
        }
    }

    private string _highway = "";
    public string Highway{
        get{
            return _highway;
        }
        set{
            if(value is not null){
                _highway = value;
            }
        }
    }

    private string _surface = "";
    public string Surface{
        get{
            return _surface;
        }
        set{
            if(value is not null){
                _surface = value;
            }
        }
    }
}
