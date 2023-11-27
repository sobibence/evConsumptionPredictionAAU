using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

[TableName("vehicle_trip_status")]
public class VehicleTripStatus : BaseEntity
{
    [ColumnName("vehicle_id")]
    public int VehicleId { get; set; }

    [ColumnName("additional_weight_kg")]
    public int AdditionalWeightKg { get; set; }

    [ColumnName("vehicle_milage_meters")]
    public int VehicleMilageMeters { get; set; }
}
