namespace EVCP.MapLoader;

public class Edge
{
    public long StartNodeId {get;set;}
    public long EndNodeId {get;set;}
    public Node StartNode {
        get{
            if(StartNode == null){
                //TODO: attempt to get it from db if the id exist
                throw new NullReferenceException();
            }
            return StartNode;
        }
    
        set
        {
            if(StartNodeId == 0){
                StartNodeId = value.NodeIdOsm;
            }

            StartNode = value;
        }
    }
    public Node EndNode {
        get{
            if(EndNode == null){
                //TODO: attempt to get it from db if the id exist
                throw new NullReferenceException();
            }
            return EndNode;
        }
        set
        {
            if(EndNodeId == 0){
                EndNodeId = value.NodeIdOsm;
            }
            EndNode = value;
            
        }
    }
    public long OsmWayId{get;set;}

    //if we already calcuted it once then we dont need to do it again
    private double length;
    public double Length{
        get{
            if (length == 0){
                length = GpsDistanceCalculator.CalculateDistance(StartNode,EndNode);
            }
            return length;
        }
        set{
            length = value;
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
            return _highway;
        }
        set{
            if(value is not null){
                _highway = value;
            }
        }
    }
}
