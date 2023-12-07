namespace EVCP.Domain.Models
{
    public class WeatherData
    {
        public WeatherData(DateTime at, bool isRaining, bool isSnowing, float windSpeed, int windDirection, float fog, float humidity, float temperature, int precipitation)
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
        public float WindSpeed { get; }
        public int WindDirection { get; }
        public float Fog { get; }
        public float Humidity { get; }
        public float Temperature { get; }
        public int Precipitation { get; }
    }
}
