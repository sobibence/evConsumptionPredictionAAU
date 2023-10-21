namespace EVCP.Domain.Models;

public class VehicleModel : BaseEntity
{
    public int BatterySizeWh { get; set; }

    public float RollingResistance { get; set; }

    public float DragCoefficient { get; set; }

    public float FrontalSize { get; set; }

    public int WeightKg { get; set; }

    public float AvgConsumptionWhKm { get; set; }

    public string Name { get; set; }

    public int AcPower { get; set; }

    public int Power { get; set; }

    public int ProducerId { get; set; }

    public int Year { get; set; }

    public float PtEfficiency { get; set; }
}
