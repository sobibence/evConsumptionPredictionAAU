using EVCP.Domain.Models;

namespace EVCP.Domain.Services
{
    public static class WeatherDataGenerator
    {
        private static readonly Random random = new();

        public static WeatherData GenerateWeatherData(DateTime date)
        {
            return new WeatherData
            (
                at: date,
                isRaining: random.NextDouble() < 0.3, // 30% chance of rain
                isSnowing: random.NextDouble() < 0.1, // 10% chance of snow
                windSpeed: random.Next(0, 30), // Wind speed in kph (0 to 50)
                windDirection: random.Next(0, 360), // Wind direction in degrees (0 to 359)
                fog: random.Next(0, 10), // Fog percentage (0% to 20%)
                humidity: random.Next(20, 80), // Humidity percentage (20% to 80%)
                temperature: random.Next(-10, 40), // Temperature in Celsius (-10°C to 40°C)
                precipitation: random.Next(0, 10) // Precipitation in mm (0 to 1.0 mm)
            );
        }
    }
}
