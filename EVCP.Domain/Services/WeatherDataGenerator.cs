using EVCP.Domain.Models;

namespace EVCP.Domain.Services
{
    public class WeatherDataGenerator
    {
        private readonly Random random;
        private HourlyWeatherData previousHourData = new(0, false, false, 0, 0, 0, 0, 0);

        public WeatherDataGenerator()
        {
            random = new Random();
        }

        public WeatherData GenerateWeatherData(DateTime date)
        {
            var hourlyData = new List<HourlyWeatherData>();
            for (int i = 0; i < 24; i++)
            {
                var hourlyDatum = GenerateHourlyWeatherData(i);
                hourlyData.Add(hourlyDatum);
                previousHourData = hourlyDatum;
            }

            return new WeatherData(date, hourlyData);
        }

        private HourlyWeatherData GenerateHourlyWeatherData(int hour)
        {
            return new HourlyWeatherData
            (
                hour: hour,
                isRaining: GenerateRandomWithPersistence(previousHourData.IsRaining, 0.3),
                isSnowing: GenerateRandomWithPersistence(previousHourData.IsSnowing, 0.1),
                windSpeed: GenerateRandomWithPersistence(previousHourData.WindSpeed, 10, 0, 30),
                windDirection: GenerateRandomWithPersistence(previousHourData.WindDirection, 30, 0, 360),
                humidity: GenerateRandomWithPersistence(previousHourData.Humidity, 5, 20, 80),
                temperature: GenerateRandomWithPersistence(previousHourData.Temperature, 3, -10, 40),
                precipitation: GenerateRandomWithPersistence(previousHourData.Precipitation, 0.2, 0, 1.0)
            );
        }

        private bool GenerateRandomWithPersistence(bool previousValue, double probability)
        {
            if (random.NextDouble() < probability)
            {
                return previousValue;
            }
            return random.NextDouble() < probability;
        }

        private int GenerateRandomWithPersistence(int previousValue, int maxChange, int minValue, int maxValue)
        {
            int change = random.Next(-maxChange, maxChange + 1);
            int newValue = previousValue + change;

            if (newValue < minValue)
            {
                newValue = minValue;
            }
            else if (newValue > maxValue)
            {
                newValue = maxValue;
            }

            return newValue;
        }

        private double GenerateRandomWithPersistence(double previousValue, double maxChange, double minValue, double maxValue)
        {
            double change = (random.NextDouble() * 2 - 1) * maxChange;
            double newValue = previousValue + change;

            if (newValue < minValue)
            {
                newValue = minValue;
            }
            else if (newValue > maxValue)
            {
                newValue = maxValue;
            }

            return newValue;
        }
    }
}
