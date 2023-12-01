using EVCP.Domain.Models;

namespace EVCP.Domain.Helpers;
public class AStarSearch
{

    //Replace the heuristic with the cost function
    public static List<Node> FindPath(Node start, Node end, Func<Node, Node, double>? heuristic = null)    {
        // Initialize open and closed sets
        if(heuristic == null){
            heuristic = GpsDistanceCalculator.CalculateDistance;
        }
        var openSet = new List<Node> { start };
        var cameFrom = new Dictionary<Node, Node>();
        var gScore = new Dictionary<Node, double> { [start] = 0 };
        var fScore = new Dictionary<Node, double> { [start] = heuristic(start, end) };

        while (openSet.Any())
        {
            var current = openSet.OrderBy(node => fScore[node]).First();

            if (current == end)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);

            foreach (Node neighbor in current.ListOfConnectedNodes)
            {
                var tentativeGScore = gScore[current] + heuristic(current, neighbor);

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + heuristic(neighbor, end);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return new List<Node>(); // No path found
    }

    private static List<Node> ReconstructPath(Dictionary<Node, Node> cameFrom, Node current)
    {
        var path = new List<Node> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }

    public static List<Edge> ConvertNodeListToEdgeList(List<Node> nodeList)
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

}
