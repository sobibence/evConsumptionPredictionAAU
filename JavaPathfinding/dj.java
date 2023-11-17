import java.util.*;
import java.io.*;

public class DijkstraAlgorithm {

    // Dijkstra's algorithm to find shortest path from source to all other nodes
    public static int[] dijkstra(int[][] graph, int source) {
        int vertices = graph.length;

        // distance used to store the distance of vertex from a source
        int[] distance = new int[vertices];

        // Boolean array to mark the vertex which is finalized
        boolean[] visited = new boolean[vertices];

        // Array to store the shortest path tree
        int[] prevNode = new int[vertices];

        // Initialize all the distance to infinity and prevNode to -1
        Arrays.fill(distance, Integer.MAX_VALUE);
        Arrays.fill(prevNode, -1);

        // Distance to the source from itself is 0
        distance[source] = 0;

        for (int i = 0; i < vertices - 1; i++) {
            // Find the vertex with the minimum distance
            int minVertex = findMinimumVertex(distance, visited);
            visited[minVertex] = true;

            // Explore all the adjacent vertices of current min vertex and update the keys
            for (int j = 0; j < vertices; j++) {
                if (graph[minVertex][j] != 0 && !visited[j] && distance[minVertex] != Integer.MAX_VALUE) {
                    int newDistance = distance[minVertex] + graph[minVertex][j];
                    if (newDistance < distance[j]) {
                        distance[j] = newDistance;
                        prevNode[j] = minVertex;
                    }
                }
            }
        }

        // Return the previous node array to reconstruct paths
        return prevNode;
    }

    // Utility function to find the vertex with the minimum distance
    public static int findMinimumVertex(int[] distance, boolean[] visited) {
        int minKey = Integer.MAX_VALUE;
        int minIndex = -1;
        for (int i = 0; i < distance.length; i++) {
            if (!visited[i] && distance[i] < minKey) {
                minKey = distance[i];
                minIndex = i;
            }
        }
        return minIndex;
    }

    // Utility function to reconstruct and print the path
    public static void printPath(int[] prevNode, int source, int destination) {
        List<Integer> path = new ArrayList<>();
        for (int at = destination; at != -1; at = prevNode[at]) {
            path.add(at);
        }
        Collections.reverse(path);
        
        if (path.get(0) != source) {
            System.out.println("No path from " + source + " to " + destination);
        } else {
            System.out.println("Path from " + source + " to " + destination + ": " + path);
        }
    }

    public static void main(String[] args) throws FileNotFoundException {
        if (args.length < 3) {
            System.out.println("Usage: java DijkstraAlgorithm <csv_file_path> <source_vertex> <destination_vertex>");
            return;
        }

        String csvFilePath = args[0];
        int source = Integer.parseInt(args[1]);
        int destination = Integer.parseInt(args[2]);

        int[][] graph = readGraphFromCSV(csvFilePath);
        int[] prevNode = dijkstra(graph, source);
        printPath(prevNode, source, destination);
    }

    // Utility function to read the graph from a CSV file
    public static int[][] readGraphFromCSV(String filePath) throws FileNotFoundException {
        List<int[]> rows = new ArrayList<>();
        try (Scanner scanner = new Scanner(new File(filePath))) {
            while (scanner.hasNextLine()) {
                String[] values = scanner.nextLine().split(",");
                int[] row = new int[values.length];
                for (int i = 0; i < values.length; i++) {
                    row[i] = Integer.parseInt(values[i].trim());
                }
                rows.add(row);
            }
        }
        return rows.toArray(new int[rows.size()][]);
    }
}
