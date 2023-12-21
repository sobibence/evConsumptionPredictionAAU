using Dapper;
using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EVCP.Domain.Repositories;

public interface IEdgeRepository : IBaseRepository<Edge>
{
    public Task<Edge?> GetByAttributesAsync(long startNodeId, long endNodeId, long edgeInfoId);
}

public class EdgeRepository : BaseRepository<Edge>, IEdgeRepository
{
    private readonly ILogger<EdgeRepository> _logger;
    private readonly DapperContext _context;

    public EdgeRepository(ILogger<EdgeRepository> logger, DapperContext context) : base(logger, context)
    {
        _logger = logger;
        _context = context;
    }
    public async Task<Edge?> GetByAttributesAsync(long startNodeId, long endNodeId, long edgeInfoId)
    {
        var parameters = new { StartNodeId = startNodeId, EndNodeId = endNodeId, EdgeInfoId = edgeInfoId };

        var columns = string.Join(", ", GetForSelect());
        var query = $"SELECT {columns} " +
                    $"FROM {Table} " +
                    $"WHERE start_node_id=@StartNodeId AND end_node_id=@EndNodeId AND edge_info_id=@EdgeInfoId;";

        //using var connection = _context.CreateConnection();

        Connection.Open();

        var result = await Connection.QueryFirstOrDefaultAsync<Edge>(query, parameters);

        Connection.Close();

        return result;
    }
}