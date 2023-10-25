using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

[TableName("fact_estimated_consumption")]
public class FactEstimatedConsumption : BaseEntity
{
    [ColumnName("edge_id")]
    public int EdgeId { get; set; }

    [ColumnName("day_in_year")]
    public int DayInYear { get; set; }

    [ColumnName("minute_in_day")]
    public int MinuteInDay { get; set; }

    [ColumnName("vehicle_id")]
    public int VehicleId { get; set; }

    [ColumnName("weather_id")]
    public int WeatherId { get; set; }

    [ColumnName("energy_consumption_wh")]
    public float EnergyConsumptionWh { get; set; }
}
