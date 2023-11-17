namespace EVCP.Domain.Models
{
    public class WeatherData
    {
        public WeatherData(DateTime at, bool isRaining, bool isSnowing, double windSpeed, int windDirection, double fog, double humidity, double temperature, double precipitation)
        {
            At = at;
            IsRaining = isRaining;
            IsSnowing = isSnowing;
            WindSpeed = windSpeed;
            WindDirection = windDirection;
            Fog = fog;
            Humidity = humidity;
            Temperature = temperature;
            Precipitation = precipitation;
        }

        public DateTime At { get; }
        public bool IsRaining { get; }
        public bool IsSnowing { get; }
        public double WindSpeed { get; }
        public int WindDirection { get; }
        public double Fog { get; }
        public double Humidity { get; }
        public double Temperature { get; }
        public double Precipitation { get; }
    }
}
