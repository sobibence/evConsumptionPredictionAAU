using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

[TableName("fact_recorded_travel")]
public class FactRecordedTravel
{
    public float speed_km_per_hour { get; set; }

    public int weather_id { get; set; }

    public int edge_id { get; set; }

    public float edge_percent { get; set; }

    public DateTimeOffset time_epoch { get; set; }

    public float acceleration_metre_per_second_squared { get; set; }

    public float energy_consumption_wh { get; set; }

    public int vehicle_id { get; set; }
}
