namespace EVCP.Domain.Models;

public class VehicleTripStatus : BaseEntity
{
    public int VehicleModelId { get; set; }

    public int AdditionalWeightKg { get; set; }

    public int VehicleMilageMeters { get; set; }

    //public int DriverAggresiveness { get; set; }
}
