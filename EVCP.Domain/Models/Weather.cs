using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

public class Weather : BaseEntity
{
    [ColumnName("temperature_celcius")]
    public float TemperatureCelsius { get; set; }

    [ColumnName("wind_km_ph")]
    public float WindKmPh { get; set; }

    [ColumnName("wind_direction_degrees")]
    public int WindDirectionDegrees { get; set; }

    [ColumnName("fog_percent")]
    public float FogPercent { get; set; }

    [ColumnName("sunshine_w_m")]
    public float SunshineWM { get; set; }

    [ColumnName("rain_mm")]
    public int RainMm { get; set; }

    [ColumnName("road_quality")]
    public int RoadQuality { get; set; }

    [EnumType]
    [ColumnName("road_type")]
    public RoadType RoadType { get; set; }
}
