using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

[TableName("fact_recorded_travel")]
public class FactRecordedTravel
{
    [ColumnName("speed_km_per_hour")]
    public float SpeedKmph { get; set; }

    [ColumnName("weather_id")]
    public int WeatherId { get; set; }

    [ColumnName("trip_id")]
    public int TripId { get; set; }

    [ColumnName("edge_id")]
    public int EdgeId { get; set; }

    [ColumnName("edge_percent")]
    public float EdgePercent { get; set; }

    [ColumnName("time_epoch")]
    public DateTimeOffset TimeEpoch { get; set; }

    [ColumnName("acceleration_metre_per_second_squared")]
    public float AccelerationMeterPerSecondSquared { get; set; }

    [ColumnName("energy_consumption_wh")]
    public float EnergyConsumptionWh { get; set; }

    [ColumnName("vehicle_id")]
    public int VehicleId { get; set; }
}
