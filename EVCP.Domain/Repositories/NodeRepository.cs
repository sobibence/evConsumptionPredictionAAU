using Dapper;
using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Helpers;
using EVCP.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EVCP.Domain.Repositories;

public interface INodeRepository : IBaseRepository<Node>
{
    public Task<Dictionary<long, Node>> GetConstructedSubGraphASyncNodeDict(Node start, Node finish, double bufferfactor = 0.2);
}

public class NodeRepository : BaseRepository<Node>, INodeRepository
{
    private readonly ILogger<NodeRepository> _logger;
    private readonly DapperContext _context;

    public NodeRepository(ILogger<NodeRepository> logger, DapperContext context) : base(logger, context)
    {
        _logger = logger;
        _context = context;
        SqlMapper.AddTypeHandler(new PointTypeMapper());
    }

    // public async Task<IEnumerable<Node>> GetSubGraphAsync(Node node1, Node node2, double bufferfactor = 0.2)
    // {
    // var columnArr = GetForSelect();
    // var columns = string.Join(", ", columnArr);
    // double distanceInRad = GpsDistanceCalculator.CalculateDistance(node1, node2) * (1 + bufferfactor);

    // double Latitude = node1.Latitude + node2.Latitude / 2;
    // double Longitude = node2.Longitude + node2.Longitude / 2;

    // DynamicParameters parameters = new DynamicParameters();
    // parameters.Add("@Latitude", Latitude);
    // parameters.Add("@Longitude", Longitude);
    // parameters.Add("@Distance", distanceInRad);

    // var query = $"SELECT {columns} FROM {Table} " +
    //             $"WHERE ST_DWITHIN({Table}.gps_coords, ST_POINT(@Latitude, @Longitude, 4326), @Distance);";

    // using var connection = _context.CreateConnection();
    // connection.Open();

    // return await connection.QueryAsync<Node>(query, parameters);

    public async Task<Dictionary<long, Node>> GetConstructedSubGraphASyncNodeDict(Node start, Node finish, double bufferfactor = 0.2)
    {

        using var connection = _context.CreateConnection();
        connection.Open();
        double distanceInRad = GpsDistanceCalculator.CalculateDistance(start, finish) * (1 + bufferfactor);

        double Latitude = (start.Latitude + finish.Latitude) / 2;
        double Longitude = (start.Longitude + finish.Longitude) / 2;

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Latitude", Latitude);
        parameters.Add("@Longitude", Longitude);
        parameters.Add("@Distance", distanceInRad);
        string queryString = @"
            SELECT  e.id, e.start_node_id, e.end_node_id, e.edge_info_id, 
                    node.id, node.gps_coords, node.osm_node_id,
                    ei.id, ei.speed_limit_kmph, ei.average_speed_kmph, ei.osm_way_id, ei.street_name, ei.surface, ei.highway
	            FROM edge e INNER JOIN node node
	            	ON e.start_node_id = node.osm_node_id 
	            		INNER JOIN edge_info ei 
	            		ON e.edge_info_id = ei.osm_way_id
	            WHERE ST_DWITHIN(node.gps_coords, ST_SETSRID(ST_MAKEPOINT(@Longitude, @Latitude), 4326), @Distance) LIMIT 100;
	        ; 
        ";
        var nodeLookup = new Dictionary<long, Node>();//NodeIdOsm
        var edgeInfoLookup = new Dictionary<long, EdgeInfo>();//OsmWayId
        var edges = connection.QueryAsync<Edge, Node, EdgeInfo, Edge>(queryString,
            (edge, node, edgeinfo) =>
            {
                Node nodeStart;
                Console.WriteLine(node.ToString());
                if (!nodeLookup.TryGetValue(node.NodeIdOsm, out nodeStart))
                {
                    nodeLookup.Add(node.NodeIdOsm, nodeStart = node);
                }
                EdgeInfo edgeinfo1;
                if (!edgeInfoLookup.TryGetValue(edgeinfo.OsmWayId, out edgeinfo1))
                {
                    edgeInfoLookup.Add(edgeinfo.OsmWayId, edgeinfo1 = edgeinfo);
                }
                Edge edge1 = edge;
                edge1.EdgeInfo = edgeinfo1;
                edge1.StartNode = nodeStart;
                return edge1;
            }, parameters);

        //adding endnodes
        List<Edge> edgeList = (List<Edge>)await edges;
        foreach (Edge edge in edgeList)
        {
            Node nodeEnd;
            if (nodeLookup.TryGetValue(edge.EndNodeId, out nodeEnd))
            {
                edge.EndNode = nodeEnd;
                edge.StartNode.ListOfConnectedNodes.Add(nodeEnd);
                edge.StartNode.ListOfConnectedEdges.Add(edge);
            }
            else
            {
                //edgeList.Remove(edge);
            }

        }


        return nodeLookup;
    }



    //Doesnt work
    public async Task<Node> GetClosestNodeToCoords(double Longitude, double Latitude)
    {
        var columnArr = GetForSelect();
        var columns = string.Join(", ", columnArr);

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Latitude", Latitude);
        parameters.Add("@Longitude", Longitude);

        var query = $"SELECT {columns} FROM {Table} " +
                    $"WHERE ST_ClosestPoint({Table}.gps_coords, ST_POINT(@Latitude, @Longitude, 4326)) = {Table}.gps_coords;";

        using var connection = _context.CreateConnection();
        connection.Open();
        return await connection.QueryFirstAsync<Node>(query, parameters);


    }




}
