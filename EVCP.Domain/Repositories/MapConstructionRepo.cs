using Dapper;
using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Helpers;
using EVCP.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EVCP.Domain.Repositories;

public interface IMapConstructionRepository
{
   public Task<IEnumerable<Edge>> GetConstructedSubGraphASync(Node start, Node finish, double bufferfactor = 0.2);
   //public Task<Dictionary<long, Node>> GetConstructedSubGraphASyncNodeDict(Node start, Node finish, double bufferfactor = 0.2);
}

public class MapConstructionRepository : IMapConstructionRepository
{
    private readonly ILogger<IMapConstructionRepository> _logger;
    private readonly DapperContext _context;

    public MapConstructionRepository(ILogger<IMapConstructionRepository> logger, DapperContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IEnumerable<Edge>> GetConstructedSubGraphASync(Node start, Node finish, double bufferfactor = 0.2){
        using var connection = _context.CreateConnection();
        connection.Open();
        double distanceInMeters = GpsDistanceCalculator.CalculateDistance(start, finish) * (1 + bufferfactor);

        double Latitude = (start.Latitude + finish.Latitude) / 2;
        double Longitude = (start.Longitude + finish.Longitude) / 2;

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Latitude", Latitude);
        parameters.Add("@Longitude", Longitude);
        parameters.Add("@Distance", distanceInMeters);
        _logger.LogInformation("Latitude: "+ Latitude.ToString());
        _logger.LogInformation("Longitude: "+Longitude.ToString());
        _logger.LogInformation("distanceInMeters: "+distanceInMeters.ToString());
        string queryString = @"
            SELECT  n.id , n.gps_coords, n.osm_node_id , ei.*,  n1.id, n1.gps_coords, n1.osm_node_id, e.*
	            FROM edge e INNER JOIN node n
	            	ON e.start_node_id = n.osm_node_id 
	            		INNER JOIN edge_info ei 
	            		ON e.edge_info_id = ei.osm_way_id
                        INNER JOIN node n1 ON e.end_node_id = n1.osm_node_id
	            WHERE ST_DWITHIN(n.gps_coords, ST_SETSRID(ST_MAKEPOINT(@Longitude, @Latitude), 4326), @Distance);
	        ; 
        ";
        var nodeLookup = new Dictionary<long, Node>();
        var edgeInfoLookup = new Dictionary<long, EdgeInfo>();
        
        var edges = connection.QueryAsync<Node, EdgeInfo, Node, Edge, Edge>(queryString, 
            (node, edgeinfo, node1, edge) => {
                Node nodeStart;
                if(!nodeLookup.TryGetValue(node.Id, out nodeStart)){
                    nodeLookup.Add(node.Id, nodeStart = node );
                }
                Node nodeEnd; 
                if(!nodeLookup.TryGetValue(node1.Id, out nodeEnd)){
                    nodeLookup.Add(node1.Id, nodeEnd = node1 );
                }
                EdgeInfo edgeinfo1;
                if(!edgeInfoLookup.TryGetValue(edgeinfo.OsmWayId, out edgeinfo1)){
                    edgeInfoLookup.Add(edgeinfo.OsmWayId, edgeinfo1 = edgeinfo);
                }
                Edge edge1 = edge;
                edge1.EdgeInfo = edgeinfo1;
                edge1.StartNode = nodeStart;
                edge1.EndNode = nodeEnd;
                nodeStart.ListOfConnectedNodes.Add(nodeEnd);
                nodeStart.ListOfConnectedEdges.Add(edge1);
                return edge1;
        },parameters, splitOn:"osm_node_id, highway, osm_node_id");
        return await edges;
    }

    // public async Task<Dictionary<long, Node>> GetConstructedSubGraphASyncNodeDict(Node start, Node finish, double bufferfactor = 0.2){
        
    //     using var connection = _context.CreateConnection();
    //     connection.Open();
    //     double distanceInRad = GpsDistanceCalculator.CalculateDistance(start, finish) * (1 + bufferfactor);

    //     double Latitude = start.Latitude + finish.Latitude / 2;
    //     double Longitude = start.Longitude + finish.Longitude / 2;

    //     DynamicParameters parameters = new DynamicParameters();
    //     parameters.Add("@Latitude", Latitude);
    //     parameters.Add("@Longitude", Longitude);
    //     parameters.Add("@Distance", distanceInRad);
    //     string queryString = @"
    //         SELECT  n.*, ei.*,  n1.*, e.*
	//             FROM edge e INNER JOIN node n
	//             	ON e.start_node_id = n.osm_node_id 
	//             		INNER JOIN edge_info ei 
	//             		ON e.edge_info_id = ei.osm_way_id
    //                     INNER JOIN node n1 ON e.end_node_id = n1.osm_node_id
	//             WHERE ST_DWITHIN(n.gps_coords, ST_POINT(@Latitude, @Longitude, 4326), @Distance);
	//         ; 
    //     ";
    //     var nodeLookup = new Dictionary<long, Node>();//nodeIDosm
    //     var edgeInfoLookup = new Dictionary<long, EdgeInfo>();
    //     var edges = connection.QueryAsync<Node, EdgeInfo, Node, Edge, Edge>(queryString, 
    //         (node, edgeinfo, node1, edge) => {
    //             Node nodeStart;
    //             if(!nodeLookup.TryGetValue(node.NodeIdOsm, out nodeStart)){
    //                 nodeLookup.Add(node.NodeIdOsm, nodeStart = node );
    //             }
    //             Node nodeEnd; 
    //             if(!nodeLookup.TryGetValue(node1.NodeIdOsm, out nodeEnd)){
    //                 nodeLookup.Add(node1.NodeIdOsm, nodeEnd = node );
    //             }
    //             EdgeInfo edgeinfo1;
    //             if(!edgeInfoLookup.TryGetValue(edgeinfo.OsmWayId, out edgeinfo1)){
    //                 edgeInfoLookup.Add(edgeinfo.OsmWayId, edgeinfo1 = edgeinfo);
    //             }
    //             Edge edge1 = edge;
    //             edge1.EdgeInfo = edgeinfo1;
    //             edge1.StartNode = nodeStart;
    //             edge1.EndNode = nodeEnd;
    //             nodeStart.ListOfConnectedNodes.Add(nodeEnd);
    //             nodeStart.ListOfConnectedEdges.Add(edge1);
    //             return edge1;
    //     },parameters);
    //     edges.Wait();
    //     return nodeLookup;
    //  }
}
