import java.util.*;

class Node {
    int x, y;
    Node parent;
    double gCost; // Cost from start to current node
    double hCost; // Heuristic cost from current node to end
    double fCost; // Total cost (gCost + hCost)

    public Node(int x, int y) {
        this.x = x;
        this.y = y;
        this.parent = null;
    }

    @Override
    public boolean equals(Object obj) {
        if (this == obj) return true;
        if (!(obj instanceof Node)) return false;
        Node node = (Node) obj;
        return x == node.x && y == node.y;
    }

    @Override
    public int hashCode() {
        return Objects.hash(x, y);
    }
}

public class AStarAlgorithm {

    private static double calculateHeuristic(Node node, Node endNode) {
        return Math.sqrt(Math.pow(node.x - endNode.x, 2) + Math.pow(node.y - endNode.y, 2));
    }

    private static List<Node> getNeighbors(Node node, int[][] grid) {
        List<Node> neighbors = new ArrayList<>();
        int[] dx = {-1, 1, 0, 0};
        int[] dy = {0, 0, -1, 1};

        for (int i = 0; i < 4; i++) {
            int newX = node.x + dx[i];
            int newY = node.y + dy[i];

            if (newX >= 0 && newX < grid.length && newY >= 0 && newY < grid[0].length && grid[newX][newY] == 1) {
                neighbors.add(new Node(newX, newY));
            }
        }
        return neighbors;
    }

    public static List<Node> findPath(int[][] grid, Node startNode, Node endNode) {
        PriorityQueue<Node> openList = new PriorityQueue<>(Comparator.comparingDouble(n -> n.fCost));
        Set<Node> closedList = new HashSet<>();

        startNode.gCost = 0;
        startNode.hCost = calculateHeuristic(startNode, endNode);
        startNode.fCost = startNode.gCost + startNode.hCost;
        openList.add(startNode);

        while (!openList.isEmpty()) {
            Node currentNode = openList.poll();
            closedList.add(currentNode);

            if (currentNode.equals(endNode)) {
                return reconstructPath(currentNode);
            }

            for (Node neighbor : getNeighbors(currentNode, grid)) {
                if (closedList.contains(neighbor)) continue;

                double tentativeGCost = currentNode.gCost + 1; // Assuming cost between nodes is 1

                if (tentativeGCost < neighbor.gCost || !openList.contains(neighbor)) {
                    neighbor.parent = currentNode;
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = calculateHeuristic(neighbor, endNode);
                    neighbor.fCost = neighbor.gCost + neighbor.hCost;

                    if (!openList.contains(neighbor)) {
                        openList.add(neighbor);
                    }
                }
            }
        }

        return new ArrayList<>(); // Path not found
    }

    private static List<Node> reconstructPath(Node endNode) {
        List<Node> path = new ArrayList<>();
        Node currentNode = endNode;
        while (currentNode != null) {
            path.add(currentNode);
            currentNode = currentNode.parent;
        }
        Collections.reverse(path);
        return path;
    }

    public static void main(String[] args) {
        int[][] grid = {
            {1, 1, 1, 1, 1},
            {1, 0, 0, 1, 1},
            {1, 1, 1, 1, 1},
            {1, 1, 1, 0, 1}
        };

        Node startNode = new Node(0, 0);
        Node endNode = new Node(3, 4);

        List<Node> path = findPath(grid, startNode, endNode);

        if (path.isEmpty()) {
            System.out.println("No path found.");
        } else {
            System.out.println("Path found:");
            for (Node node : path) {
                System.out.printf("(%d, %d) ", node.x, node.y);
            }
        }
    }
}

