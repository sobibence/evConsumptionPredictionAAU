using System.ComponentModel;
using System.Reflection;
using System.Xml;
using EVCP.Domain;
using EVCP.Domain.Models;

namespace EVCP.Simulation;

public class Car
{
    private float currentSpeed = 0; // m/s

    public float CurrentSpeed { get; }

    public float CurrentAccel { get; }
    static private readonly float decelMax = -5; //m/s*s
    static private readonly float accelMax = 3; //m/s*s
    static private readonly float intersectionSpeed = 4; // m/s
    private float currentAccel = 0;
    private int currentEdgeIdx = 0;
    private List<Edge> route;
    private CarThreadClass parent;
    private float currentEdgePercent = 0;

    public Edge CurrentEdge { get { return route[currentEdgeIdx]; } }
    private bool start = true;

    private bool finish = false;
    public Car(List<Edge> edges, CarThreadClass carThreadClass)
    {
        route = edges;
        parent = carThreadClass;
    }


    public List<TripData> getNextCarStatus(float dt)
    {
        List<TripData> tripDataList = new();
        
        if (finish)
        {
            parent.RequestNewRouteForCar(this);
            return tripDataList;
        }
        if (start)
        {
            TripData tripData1 = new()
            {
                Edge = route.First(),
                Time = DateTime.Now,
                Weather = new()
            };
            start = false;
            tripDataList.Add(tripData1);
        }//-- set decel or accel

        if (distanceUntilNextIntersection(currentEdgeIdx, currentEdgePercent) < getBrakingDistanceFromSpeed(currentSpeed))
        {
            currentAccel = decelMax;
        }
        else if (getGoalSpeedinMeterPerS() > currentSpeed)
        {
            currentAccel = accelMax;

        }
        //-- if we reach the goal speed in this timeframe then we stop the acceleration midway..
        float timeDelta = dt / 1000; //convert to secs
        float timeAccelerating = 0f;
        if (currentAccel == decelMax)
        {
            //if(currentSpeed + decelMax*timeDelta < intersectionSpeed){
            timeAccelerating = (intersectionSpeed - currentSpeed) / decelMax;
            timeDelta = timeDelta - timeAccelerating;
            //}
        }
        if (currentAccel == accelMax)
        {
            //if(currentSpeed + accelMax*timeDelta > getGoalSpeedinMeterPerS()){
            timeAccelerating = (getGoalSpeedinMeterPerS() - currentSpeed) / accelMax;
            timeDelta = timeDelta - timeAccelerating;
            //}
        }

        if (timeAccelerating > dt / 1000)
        {
            timeAccelerating = dt / 1000;
            timeDelta = 0f;
        }
        double distanceTravelled = currentSpeed * timeAccelerating + 0.5 * currentAccel * timeAccelerating * timeAccelerating;
        //TODO: fix Time: timestamps are by computation it should be by deltat
        currentSpeed = currentSpeed + currentAccel * timeAccelerating;
        distanceTravelled = distanceTravelled + timeDelta * currentSpeed;
        

        TripData tripData = new()
        {
            Edge = route.First(),
            Time = DateTime.Now,
            Weather = new(),
            EdgePercent = currentEdgePercent,
            Acceleration = currentAccel,
            Speed = currentSpeed
        };
        tripDataList.Add(tripData);
        moveCarInMeters(distanceTravelled, tripDataList);
        TripData tripData2 = new()
        {
            Edge = route.First(),
            Time = DateTime.Now,
            Weather = new(),
            EdgePercent = currentEdgePercent,
            Acceleration = currentAccel,
            Speed = currentSpeed
        };
        tripDataList.Add(tripData2);
        if (finish)
        {
            TripData tripData1 = new()
            {
                Edge = route.First(),
                Time = DateTime.Now,
                Weather = new(),
                EdgePercent = 1f
            };
            tripDataList.Add(tripData1);
        }

        return tripDataList;
    }

    public void Reset(List<Edge> edges)
    {
        route = edges;
        currentAccel = 0f;
        currentEdgeIdx = 0;
        currentEdgePercent = 0f;
        currentSpeed = 0f;
        start = true;
        finish = false;
    }

    private void moveCarInMeters(double distanceTravelled, List<TripData> tripDataList)
    {
        if (finish)
        {
            return;
        }
        double remainderFromCurrentEdge = CurrentEdge.Length * (1 - currentEdgePercent);
        if (remainderFromCurrentEdge > distanceTravelled)
        {
            currentEdgePercent = currentEdgePercent + (float)(distanceTravelled / CurrentEdge.Length);
            return;
        }
        else
        {
            currentEdgeIdx++;
            if (currentEdgeIdx == route.Count)
            {
                finish = true;
                return;
            }
            TripData tripData1 = new()
            {
                Edge = CurrentEdge,
                Time = DateTime.Now,
                Weather = new()
            };
            tripDataList.Add(tripData1);
            moveCarInMeters(distanceTravelled - remainderFromCurrentEdge, tripDataList);
        }
    }

    private bool isIntersection(Edge original)
    {
        if (original.EndNode.ListOfConnectedEdges.Count > 2)
        {
            return true;
        }
        return false;
    }

    private double distanceUntilNextIntersection(int currentIdx, float percent)
    {
        double distance = 0f;
        int intersectionIdx = currentIdx;
        bool found = false;
        while (!found && intersectionIdx < route.Count)
        {
            if (!isIntersection(route[intersectionIdx]))
            {
                found = false;

                intersectionIdx++;
            }
            else
            {
                found = true;
            }
            if (currentIdx == intersectionIdx)
            {
                distance += route[intersectionIdx].Length * (1 - currentEdgePercent);
            }
            else
            {
                distance += route[intersectionIdx].Length;
            }
        }
        //Console.WriteLine("distance to next intersection:" + distance.ToString());
        return distance;
    }

    private double getBrakingDistanceFromSpeed(float velocity)
    {
        return ((intersectionSpeed * intersectionSpeed) - (velocity * velocity)) / 2 * decelMax;
    }

    private float getGoalSpeedinMeterPerS()
    {
        float goalspeed = 50 / 3.6f;
        if (route[currentEdgeIdx].EdgeInfo.SpeedLimit != 0)
        {
            goalspeed = route[currentEdgeIdx].EdgeInfo.SpeedLimit / 3.6f;
        }
        return goalspeed;
    }

}
