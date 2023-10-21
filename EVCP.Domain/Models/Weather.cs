using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

public class Weather : BaseEntity
{
    public float temperature_celcius { get; set; }

    public float wind_km_ph { get; set; }

    public int wind_direction_degrees { get; set; }

    public float fog_percent { get; set; }

    public float sunshine_w_m { get; set; }

    public int rain_mm { get; set; }

    public int road_quality { get; set; }

    [EnumType]
    public road_type road_type { get; set; }
}
