namespace EVCP.Domain.Models;

public class FactEstimatedConsumption : BaseEntity
{
    public int EdgeId { get; set; }

    public int DayInYear { get; set; }

    public int MinuteInDay { get; set; }

    public int VehicleId { get; set; }

    public int WeatherId { get; set; }

    public float EnergyConsumptionWh { get; set; }
}
