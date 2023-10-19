using EVCP.Domain.Models;

namespace EVCP.Simulation;
public class AStarSearch
{
    public static List<Node> FindPath(Node start, Node end)    {
        // Initialize open and closed sets
        var openSet = new List<Node> { start };
        var cameFrom = new Dictionary<Node, Node>();
        var gScore = new Dictionary<Node, double> { [start] = 0 };
        var fScore = new Dictionary<Node, double> { [start] = Heuristic(start, end) };

        while (openSet.Any())
        {
            var current = openSet.OrderBy(node => fScore[node]).First();

            if (current == end)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);

            foreach (Node neighbor in current.ListOfConnectedNodes)
            {
                var tentativeGScore = gScore[current] + GpsDistanceCalculator.CalculateDistance(current, neighbor);

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, end);

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

    private static double Heuristic(Node node, Node goal)
    {
        // You can use different heuristics here, like Euclidean distance or Manhattan distance
        return GpsDistanceCalculator.CalculateDistance(node, goal);
    }

}
