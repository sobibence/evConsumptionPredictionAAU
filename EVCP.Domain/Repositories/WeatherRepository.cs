using Dapper;
using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EVCP.Domain.Repositories;

public interface IWeatherRepository : IBaseRepository<Weather>
{
    public Task<Weather?> GetByMatchingAttributes(float temperature, float windSpeed, int windDirection, float fogPercent, int rainMm);
}

public class WeatherRepository : BaseRepository<Weather>, IWeatherRepository
{
    private readonly ILogger<WeatherRepository> _logger;

    public WeatherRepository(ILogger<WeatherRepository> logger, DapperContext context) : base(logger, context)
    {
        _logger = logger;
    }

    public async Task<Weather?> GetByMatchingAttributes(float temperature, float windSpeed, int windDirection, float fogPercent, int rainMm)
    {
        var parameters = new
        {
            Temperature = temperature,
            WindSpeed = windSpeed,
            WindDirection = windDirection,
            FogPercent = fogPercent,
            RainMm = rainMm
        };
        var query = $"SELECT * FROM {Table} " +
                    $"WHERE temperature_celcius=@Temperature AND " +
                            $"wind_km_ph=@WindSpeed AND " +
                            $"wind_direction_degrees=@WindDirection AND " +
                            $"fog_percent=@FogPercent AND " +
                            $"rain_mm=@RainMm;";

        Connection.Open();

        var result = await Connection.QueryFirstOrDefaultAsync<Weather>(query, parameters);

        Connection.Close();

        return result;
    }
}
