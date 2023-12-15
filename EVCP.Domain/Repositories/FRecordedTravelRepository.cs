using Dapper;
using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EVCP.Domain.Repositories;

public interface IFRecordedTravelRepository : IBaseRepository<FactRecordedTravel>
{
    public Task<IEnumerable<FactRecordedTravel>> GetByTimestamp(DateTimeOffset from, DateTimeOffset to);

    public Task<IEnumerable<FactRecordedTravel>> GetByVehicle(int vehicleId);
}

public class FRecordedTravelRepository : BaseRepository<FactRecordedTravel>, IFRecordedTravelRepository
{
    private readonly ILogger<FRecordedTravelRepository> _logger;
    private readonly DapperContext _context;

    public FRecordedTravelRepository(ILogger<FRecordedTravelRepository> logger, DapperContext context) : base(logger, context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IEnumerable<FactRecordedTravel>> GetByTimestamp(DateTimeOffset from, DateTimeOffset to)
    {
        var parameters = new { From = from, To = to };
        var query = $"SELECT * FROM {Table} " +
                    $"WHERE time_epoch BETWEEN @From AND @TO;";

        using var connection = _context.CreateConnection();
        connection.Open();

        var result = (await connection.QueryAsync<FactRecordedTravel>(query, parameters)).ToList();
        connection.Close();
        return result;
    }

    public async Task<IEnumerable<FactRecordedTravel>> GetByVehicle(int vehicleId)
    {
        var parameters = new { VehicleId = vehicleId };
        var query = $"SELECT * FROM {Table} " +
                    $"WHERE vehicle_id=@VehicleId;";

        using var connection = _context.CreateConnection();
        connection.Open();

        var result = (await connection.QueryAsync<FactRecordedTravel>(query, parameters)).ToList();
        connection.Close();
        return result;
    }
}
