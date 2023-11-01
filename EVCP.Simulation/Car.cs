using System.Xml;
using EVCP.Domain;
using EVCP.Domain.Models;

namespace EVCP.Simulation;

public class Car
{
    private float currentSpeed = 0; // m/s
    private float decel = 2; //m/s*s
    private float accel = 1; //m/s*s

    private int currentEdgeIdx = 0;
    private List<Edge> route;

    private Edge currentEdge;

    private float currentEdgePercent = 0; 
    private bool start = true;
    public Car(List<Edge> edges){
        route = edges; 
    }

    public List<TripData> getNextCarStatus(float dt){
        List<TripData> tripDataList = new();
        TripData tripData = new();
        if(start){
            TripData tripData1 = new()
            {
                Edge = route.First(),
                Time = new(),
                Weather = new()
            };
            tripDataList.Add(tripData1);
        }


        return tripDataList;
    }

    private bool isIntersection(Edge original){
        if(original.EndNode.ListOfConnectedEdges.Count > 2){
            return true;
        }
        return false;
    }

    

}
