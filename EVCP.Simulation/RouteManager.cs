

using EVCP.Domain.Helpers;
using EVCP.Domain.Models;
using EVCP.MapLoader;

namespace EVCP.Simulation;
public class RouteManager
{
    Map map;
    Random r = new Random();
    double minDistance = 10000;
    double maxDistance = 20000;
    public RouteManager()
    {
        Task task = RequestAndInitMap();
        task.Wait();
    }
    async Task RequestAndInitMap()
    {
        string aalborgRequestString = @"
                [out:json];
                way
                [""highway""]
                (57.0040, 9.8344, 57.0827, 10.0721)
                ->.road;
                .road out geom;
                ";

        //map = await MapLoaderClass.RequestAndProcessMap(aalborgRequestString);
        map = await MapLoaderClass.ReadMapFromFile();
    }

    private List<Edge> TryGeneratingOneRoute()
    {

        Node first = GetRandomNode();
        Node second = GetRandomNode();
        int maxtries = 1000;
        bool found = false;
        for (int i = 0; i < maxtries && !found; i++)
        {
            double dist = GpsDistanceCalculator.CalculateDistance(first, second);
            if (minDistance < dist && maxDistance > dist)
            {
                found = true;
            }
            else
            {
                second = GetRandomNode();
            }
        }

        if (found)
        {
            List<Node> nodeList = AStarSearch.FindPath(first, second);

            // Console.Write(@"[out:json][timeout:25]; (node(id:");
            // foreach (Node node in nodeList)
            // {
            //     Console.Write(node.NodeIdOsm);
            //     if (nodeList.Last() != node)
            //     {
            //         Console.Write(", ");
            //     }
            // }
            // Console.WriteLine(@")({{bbox}});); out body;>;");
            if (nodeList.Any())
            {
                return ConvertNodeListToEdgeList(nodeList);
            }
        }

        return new List<Edge>();
    }

    public List<Edge> RequestRoute()
    {
        int maxtries = 10;
        List<Edge> list = TryGeneratingOneRoute();
        for (int i = 0; i < maxtries && !list.Any(); i++)
        {
            list = TryGeneratingOneRoute();
        }
        return list;
    }

    private List<Edge> ConvertNodeListToEdgeList(List<Node> nodeList)
    {
        int count = nodeList.Count;
        List<Edge> edgeList = new();
        if (count < 2){
            return edgeList;
        }
        for (int i = 0; i < count - 1; i++)
        {
            edgeList.Add(nodeList[i].ListOfConnectedEdges.First(edge => 
                edge.StartNodeId == nodeList[i+1].NodeIdOsm || edge.EndNodeId == nodeList[i+1].NodeIdOsm));
        }
        return edgeList;
    }

    private Node GetRandomNode()
    {
        return map.Nodes[r.Next(0, map.Nodes.Count)];
    }
}
