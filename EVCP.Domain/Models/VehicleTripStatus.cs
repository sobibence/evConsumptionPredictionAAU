using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

[TableName("vehicle_trip_status")]
public class VehicleTripStatus : BaseEntity
{
    public int vehicle_model_id { get; set; }

    public int additional_weight_kg { get; set; }

    public int vehicle_milage_meters { get; set; }

    //public int driver_aggresiveness { get; set; }
}
