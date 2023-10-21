namespace EVCP.Domain.Models;

public class FactRecordedTravel
{
    public float SpeedKmPerHour { get; set; }

    public int WeatherId { get; set; }

    public int EdgeId { get; set; }

    public float EdgePercent { get; set; }

    public DateTimeOffset TimeEpoch { get; set; }

    public float AccelerationMetrePerSecondSquared { get; set; }

    public float EnergyConsumptionWh { get; set; }

    public int VehicleId { get; set; }
}
