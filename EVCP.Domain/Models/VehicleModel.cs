using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

[TableName("vehicle_model")]
public class VehicleModel : BaseEntity
{
    [ColumnName("battery_size_wh")]
    public int BatterySizeWh { get; set; }

    [ColumnName("rolling_resistance")]
    public float RollingResistance { get; set; }

    [ColumnName("drag_coefficient")]
    public float DragCoefficient { get; set; }

    [ColumnName("frontal_size")]
    public float FrontalSize { get; set; }

    [ColumnName("weight_kg")]
    public int WeightKg { get; set; }

    [ColumnName("avg_consumption_wh_km")]
    public float AvgConsumptionWhKm { get; set; }

    [ColumnName("name")]
    public string Name { get; set; }

    [ColumnName("ac_power")]
    public int AcPower { get; set; }

    [ColumnName("power")]
    public int Power { get; set; }

    [ColumnName("producer_id")]
    public int ProducerId { get; set; }

    [ColumnName("year")]
    public int Year { get; set; }

    [ColumnName("pt_effeciency")]
    public float PtEfficiency { get; set; }
}
