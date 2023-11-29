using Dapper;
using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Helpers;
using EVCP.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EVCP.Domain.Repositories;

public interface INodeRepository : IBaseRepository<Node>
{

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

    public async Task<IEnumerable<Node>> GetSubGraphAsync(Node node1, Node node2, double bufferfactor = 0.2)
    {
        var columnArr = GetForSelect();
        var columns = string.Join(", ", columnArr);
        double distanceInRad = GpsDistanceCalculator.CalculateDistance(node1, node2) * (1 + bufferfactor);

        double Latitude = node1.Latitude + node2.Latitude / 2;
        double Longitude = node2.Longitude + node2.Longitude / 2;

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Latitude", Latitude);
        parameters.Add("@Longitude", Longitude);
        parameters.Add("@Distance", distanceInRad);

        var query = $"SELECT {columns} FROM {Table} " +
                    $"WHERE ST_DWITHIN({Table}.gps_coords, ST_POINT(@Latitude, @Longitude, 4326), @Distance);";

        using var connection = _context.CreateConnection();
        connection.Open();

        return await connection.QueryAsync<Node>(query, parameters);
    }




}
