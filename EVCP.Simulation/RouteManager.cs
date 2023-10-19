
using System.Reflection.Metadata;
using EVCP.Domain.Models;
using EVCP.MapLoader;

namespace EVCP.Simulation;
public class RouteManager
{
    Map map;
    Random r = new Random();
    double minDistance = 2000;
    double maxDistance = 10000;
    public RouteManager(){
        Task task = RequestAndInitMap();
        task.Wait();
    }
    async Task RequestAndInitMap(){
        string aalborgRequestString = @"
                [out:json];
                way
                [""highway""]
                (57.0040, 9.8344, 57.0827, 10.0721)
                ->.road;
                .road out geom;
                ";

       map = await MapLoaderClass.RequestAndProcessMap(aalborgRequestString);
    }

    public List<Edge> RequestRoute(){
        Node first = GetRandomNode();
        Node second = GetRandomNode();
        int maxtries = 1000;
        bool found = false;
        for(int i = 0; i < maxtries && !found; i++){
            double dist = GpsDistanceCalculator.CalculateDistance(first,second);
            if(minDistance < dist && maxDistance > dist){
                found = true;
            }
            else{
                second = GetRandomNode();
            }
        }
        if(found){
            Console.WriteLine("found 2 good nodes:");
            Console.WriteLine(first.ToString());
            Console.WriteLine(second.ToString());

        }else{
            Console.WriteLine("did not find nodes");
        }

        return new List<Edge>();
    }

    private Node GetRandomNode()
    {
        return map.Nodes[r.Next(0, map.Nodes.Count)];
    }
}
