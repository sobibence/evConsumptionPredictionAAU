using Dapper;
using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EVCP.Domain.Repositories;

public interface IFEstConsumptionRepository : IBaseRepository<FactEstimatedConsumption>
{
    public Task<IEnumerable<FactEstimatedConsumption>> GetByEdge(int edgeId);
}

public class FEstConsumptionRepository : BaseRepository<FactEstimatedConsumption>, IFEstConsumptionRepository
{
    private readonly ILogger<FEstConsumptionRepository> _logger;
    private readonly DapperContext _context;

    public FEstConsumptionRepository(ILogger<FEstConsumptionRepository> logger, DapperContext context) : base(logger, context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IEnumerable<FactEstimatedConsumption>> GetByEdge(int edgeId)
    {
        var parameters = new { EdgeId = edgeId };
        var query = $"SELECT * FROM {Table} " +
                    $"WHERE edge_id=@EdgeId;";

        using var connection = _context.CreateConnection();
        connection.Open();

        var result = (await connection.QueryAsync<FactEstimatedConsumption>(query, parameters)).ToList();

        return result;
    }
}
