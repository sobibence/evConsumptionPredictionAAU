using Dapper;
using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;

namespace EVCP.Domain.Repositories;

public interface IWeatherRepository : IBaseRepository<Weather>
{

}

public class WeatherRepository : BaseRepository<Weather>, IWeatherRepository
{
    private readonly DapperContext _context;

    public WeatherRepository(DapperContext context) : base(context)
    {
        _context = context;
    }

    public async override Task<bool> Create(Weather entity)
    {
        if (entity == null) return false;

        // generate insert sql query
        string columns = string.Join(", ", new string[]
        {
            "temperature_celcius", "wind_km_ph", "wind_direction_degrees", "fog_percent",
            "sunshine_w_m", "rain_mm", "road_quality", "road_type"
        });
        string values = string.Join(",", new string[]
        {
            "@temperature_celcius", "@wind_km_ph", "@wind_direction_degrees", "@fog_percent",
            "@sunshine_w_m", "@rain_mm", "@road_quality", "'asphalt'"
        });
        var query = $"INSERT INTO {Table} ({columns}) VALUES ({values})";

        using var connection = _context.CreateConnection();
        connection.Open();

        var result = await connection.ExecuteAsync(query, entity);

        return result > 0;
    }
}
