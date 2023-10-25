using EVCP.Domain.Models;
using EVCP.OpenWeather;
using Newtonsoft.Json;

namespace OpenWeather
{
    public class OpenWeatherService
    {
        private readonly string apiKey = "";

        public OpenWeatherService()
        {
        }

        public async Task<WeatherData> GetWeatherAsync(double lat, double lon)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}&units=metric";

            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = response.Content.ReadAsStringAsync().Result;

            var openWeatherResponse = JsonConvert.DeserializeObject<OpenWeatherResponse>(content);

            return Map(openWeatherResponse);
        }

        private static WeatherData Map(OpenWeatherResponse openWeatherResponse)
        {
            return new WeatherData
                (
                    at: DateTimeOffset.FromUnixTimeSeconds(openWeatherResponse.Dt).LocalDateTime,
                    isRaining: openWeatherResponse.Weather.Exists(w => w.Main == "Rain"),
                    isSnowing: openWeatherResponse.Weather.Exists(w => w.Main == "Snow"),
                    windSpeed: (int)openWeatherResponse.Wind.Speed,
                    windDirection: openWeatherResponse.Wind.Deg,
                    fog: CalculateFog(openWeatherResponse.Visibility, openWeatherResponse.Weather.Select(w => w.Id).ToArray()),
                    humidity: openWeatherResponse.Main.Humidity,
                    temperature: (int)openWeatherResponse.Main.Temp,
                    precipitation: openWeatherResponse.Rain?.ThreeHours ?? 0
                );
        }

        private static int CalculateFog(int visibility, int[] weatherCodes)
        {
            var random = new Random();

            if (visibility < 1000) // Visibility in meters
            {
                return random.Next(50, 100);
            }

            if (weatherCodes.Contains(701) || weatherCodes.Contains(741)) // Mist or fog
            {
                return random.Next(20, 100);
            }

            if (weatherCodes.Contains(721)) // Haze
            {
                return random.Next(10, 30);
            }

            return 0;
        }
    }
}