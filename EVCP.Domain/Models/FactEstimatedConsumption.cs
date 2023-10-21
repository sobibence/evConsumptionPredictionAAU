using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

[TableName("fact_estimated_consumption")]
public class FactEstimatedConsumption : BaseEntity
{
    public int edge_id { get; set; }

    public int day_in_year { get; set; }

    public int minute_in_day { get; set; }

    public int vehicle_id { get; set; }

    public int weather_id { get; set; }

    public float energy_consumption_wh { get; set; }
}
