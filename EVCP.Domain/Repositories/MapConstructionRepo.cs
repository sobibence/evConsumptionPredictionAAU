using Dapper;
using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Helpers;
using EVCP.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EVCP.Domain.Repositories;

public interface IMapConstructionRepository
{
   public Task<Dictionary<long, Node>> GetConstructedSubGraphASyncNodeDict(Node start, Node finish, double bufferfactor = 0.2);
   public Task<IEnumerable<Edge>> GetAstarRouteFromNodeOsmIds(long startNodeOsmId, long endNodeOsmId);
}

public class MapConstructionRepository : IMapConstructionRepository
{
    private readonly ILogger<IMapConstructionRepository> _logger;
    private readonly DapperContext _context;

    public MapConstructionRepository(ILogger<IMapConstructionRepository> logger, DapperContext context)
    {
        _logger = logger;
        _context = context;
        SqlMapper.AddTypeHandler(new PointTypeMapper());
    }

    public async Task<IEnumerable<Edge>> GetAstarRouteFromNodeOsmIds(long startNodeOsmId, long endNodeOsmId)
    {
        using var connection = _context.CreateConnection();
        connection.Open();
        
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@startNodeId", startNodeOsmId);
        parameters.Add("@endNodeId", endNodeOsmId);
        string queryString = @"
            SELECT 
	            e.id, e.start_node_id, e.end_node_id, e.edge_info_id, 
                nodestart.id, nodestart.gps_coords Point, nodestart.osm_node_id NodeIdOsm,
                nodeend.id, nodeend.gps_coords Point, nodeend.osm_node_id NodeIdOsm,
                ei.id, ei.speed_limit_kmph SpeedLimit, ei.average_speed_kmph, ei.osm_way_id, ei.street_name, ei.surface, ei.highway
                FROM pgr_aStar(
                    'SELECT e.id , start_node_id as source, end_node_id as target,
	                st_distance(startnode.gps_coords, endnode.gps_coords) as cost,
	                st_x(startnode.gps_coords::geometry) as x1,
	                st_y(startnode.gps_coords::geometry) y1,
	                st_x(endnode.gps_coords::geometry) as x2,
	                st_y(endnode.gps_coords::geometry) as y2 FROM edge e, node startnode, node endnode
 	                where e.end_node_id = endnode.osm_node_id and e.start_node_id = startnode.osm_node_id',
                    @startNodeId, @endNodeId) as astar 
                    inner join edge e on astar.edge = e.id
				    INNER JOIN node nodestart
	            	ON e.start_node_id = nodestart.osm_node_id 
                    inner join node nodeend 
                    on nodeend.osm_node_id = e.end_node_id 
	            	INNER JOIN edge_info ei 
	            	ON e.edge_info_id = ei.osm_way_id
				;
        ";
        var nodeLookup = new Dictionary<long, Node>();//NodeIdOsm
        var edgeInfoLookup = new Dictionary<long, EdgeInfo>();//OsmWayId
        var edges = await connection.QueryAsync< Edge, Node, Node, EdgeInfo, Edge>(queryString, 
            ( edge, node_start,node_end, edgeinfo) => {
                Node nodeStart;
                if(!nodeLookup.TryGetValue(node_start.NodeIdOsm, out nodeStart)){
                    nodeLookup.Add(node_start.NodeIdOsm, nodeStart = node_start );
                }
                Node nodeEnd;
                if(!nodeLookup.TryGetValue(node_end.NodeIdOsm, out nodeEnd)){
                    nodeLookup.Add(node_end.NodeIdOsm, nodeEnd = node_end );
                }
                EdgeInfo edgeinfo1;
                if(!edgeInfoLookup.TryGetValue(edgeinfo.OsmWayId, out edgeinfo1)){
                    edgeInfoLookup.Add(edgeinfo.OsmWayId, edgeinfo1 = edgeinfo);
                }
                Edge edge1 = edge;
                edge1.EdgeInfo = edgeinfo1;
                edge1.StartNode = nodeStart;
                edge1.EndNode = nodeEnd;
                edge1.StartNode.ListOfConnectedNodes.Add(nodeEnd);
                edge1.StartNode.ListOfConnectedEdges.Add(edge1);
                return edge1;
        },parameters,splitOn:"id,id,id");
        
        //adding endnodes
        //_logger.LogInformation(edges.Count().ToString());
        

        connection.Close();
        return edges;
    }

    public async Task<Dictionary<long, Node>> GetConstructedSubGraphASyncNodeDict(Node start, Node finish, double bufferfactor = 0.2){
        
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
                    node.id, node.gps_coords Point, node.osm_node_id NodeIdOsm,
                    ei.id, ei.speed_limit_kmph SpeedLimit, ei.average_speed_kmph, ei.osm_way_id, ei.street_name, ei.surface, ei.highway
	            FROM edge e INNER JOIN node node
	            	ON e.start_node_id = node.osm_node_id 
	            		INNER JOIN edge_info ei 
	            		ON e.edge_info_id = ei.osm_way_id
	            WHERE ST_DWITHIN(node.gps_coords, ST_SETSRID(ST_MAKEPOINT(@Longitude, @Latitude), 4326), @Distance);
	        ; 
        ";
        var nodeLookup = new Dictionary<long, Node>();//NodeIdOsm
        var edgeInfoLookup = new Dictionary<long, EdgeInfo>();//OsmWayId
        var edges =  connection.QueryAsync< Edge, Node,EdgeInfo, Edge>(queryString, 
            ( edge, node, edgeinfo) => {
                Node nodeStart;
                if(!nodeLookup.TryGetValue(node.NodeIdOsm, out nodeStart)){
                    nodeLookup.Add(node.NodeIdOsm, nodeStart = node );
                }
                EdgeInfo edgeinfo1;
                if(!edgeInfoLookup.TryGetValue(edgeinfo.OsmWayId, out edgeinfo1)){
                    edgeInfoLookup.Add(edgeinfo.OsmWayId, edgeinfo1 = edgeinfo);
                }
                Edge edge1 = edge;
                edge1.EdgeInfo = edgeinfo1;
                edge1.StartNode = nodeStart;
                return edge1;
        },parameters,splitOn:"id,id");
        
        //adding endnodes
        List<Edge> edgeList =  (List<Edge>)await edges;
        foreach(Edge edge in edgeList){
            Node nodeEnd;
            if(nodeLookup.TryGetValue(edge.EndNodeId, out nodeEnd)){
                edge.EndNode = nodeEnd;
                edge.StartNode.ListOfConnectedNodes.Add(nodeEnd);
                edge.StartNode.ListOfConnectedEdges.Add(edge);
            }

        }

        connection.Close();
        return nodeLookup;

     }
}
