using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

public class Weather : BaseEntity
{
    public float TemperatureCelsius { get; set; }

    public float WindKmPh { get; set; }

    public int WindDirectionDegrees { get; set; }

    public float FogPercent { get; set; }

    public float SunshineWM { get; set; }

    public int RainMm { get; set; }

    public int RoadQuality { get; set; }

    [EnumType]
    public string RoadType { get; set; }
}
