using EVCP.DataAccess.Repositories;

namespace EVCP.Domain.Models;

public class Weather : BaseEntity
{
    public float temperature_celsius { get; set; }

    public float wind_km_ph { get; set; }

    public wind_direction wind_direction { get; set; }
    //public string wind_direction { get; set; }

    public float fog_percent { get; set; }

    public float sunshine_w_m { get; set; }

    public int rain_mm { get; set; }

    public int road_quality { get; set; }

    public road_type road_type { get; set; }
    //public string road_type { get; set; }
}
