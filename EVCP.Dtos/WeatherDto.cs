namespace EVCP.Dtos;

public class WeatherDto
{
    public float TemperatureCelsius { get; set; }
    public float WindKph { get; set; }
    public int WindDirection { get; set; }
    public float FogPercent { get; set; }
    public int RainMm { get; set; }

    public WeatherDto(float temperatureCelsius, float windKph, int windDirection, float fogPercent, int rainMm)
    {
        TemperatureCelsius = temperatureCelsius;
        WindKph = windKph;
        WindDirection = windDirection;
        FogPercent = fogPercent;
        RainMm = rainMm;
    }
}
