using EVCP.Domain.Models;
using EVCP.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace EVCP.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly string apiKey = "";

        public WeatherController()
        {
        }

        [HttpGet]
        public WeatherData GetWeatherData(DateTime date)
        {
            var weatherDataGenerator = new WeatherDataGenerator();
            return weatherDataGenerator.GenerateWeatherData(date);
        }

        [HttpGet]
        [Route("OpenWeather")]
        public async Task<WeatherData> GetWeatherData(double lat, double lon)
        {
            using HttpClient client = new();
            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&units=metric&appid={apiKey}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    Log.Information(json);

                    return JsonConvert.DeserializeObject<WeatherData>(json);
                }
                else
                {
                    Log.Error(response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }

            return null;
        }

    }
}
