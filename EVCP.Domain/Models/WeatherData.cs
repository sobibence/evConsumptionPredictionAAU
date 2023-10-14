namespace EVCP.Domain.Models
{
    public class WeatherData
    {
        public WeatherData(DateTime date, List<HourlyWeatherData> hourlyData)
        {
            Date = date;
            HourlyData = hourlyData;
        }

        public DateTime Date { get; }
        public List<HourlyWeatherData> HourlyData { get; set; }
    }

    public class HourlyWeatherData
    {
        public HourlyWeatherData(int hour, bool isRaining, bool isSnowing, int windSpeed, int windDirection, int humidity, int temperature, double precipitation)
        {
            Hour = hour;
            IsRaining = isRaining;
            IsSnowing = isSnowing;
            WindSpeed = windSpeed;
            WindDirection = windDirection;
            Humidity = humidity;
            Temperature = temperature;
            Precipitation = precipitation;
        }

        public int Hour { get; }
        public bool IsRaining { get; }
        public bool IsSnowing { get; }
        public int WindSpeed { get; }
        public int WindDirection { get; }
        public int Humidity { get; }
        public int Temperature { get; }
        public double Precipitation { get; }
    }
}
