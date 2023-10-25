using EVCP.Domain.Models;
using EVCP.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using OpenWeather;
using Serilog;

namespace EVCP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{

    public WeatherController()
    {
    }

    [HttpGet]
    [Route("Generate")]
    public WeatherData GetWeatherData(DateTime date)
    {
        return WeatherDataGenerator.GenerateWeatherData(date);
    }

    [HttpGet]
    [Route("OpenWeather")]
    public async Task<WeatherData> GetWeatherData(double lat, double lon)
    {
        try
        {
            var openWeatherService = new OpenWeatherService();
            return await openWeatherService.GetWeatherAsync(lat, lon);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }

        return WeatherDataGenerator.GenerateWeatherData(DateTime.Now);
    }
}
