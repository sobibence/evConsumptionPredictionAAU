using EnumType = EVCP.Domain.Helpers.EnumType;

namespace EVCP.Domain.Models;

public class Weather : BaseEntity
{
    [ColumnName("temperature_celcius")]
    public float TemperatureCelsius { get; set; }

    [ColumnName("wind_km_ph")]
    public float WindSpeed { get; set; }

    [ColumnName("wind_direction_degrees")]
    public int WindDirection { get; set; }

    [ColumnName("fog_percent")]
    public float FogPercent { get; set; }

    [ColumnName("sunshine_w_m")]
    public float Sunshine { get; set; }

    [ColumnName("rain_mm")]
    public int RainMm { get; set; }
}
