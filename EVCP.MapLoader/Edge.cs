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
        {StartNode = value;}
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
        {EndNode = value;}
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
    public double SpeedLimit{get;set;}
}
