using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

[TableName("vehicle_model")]
public class VehicleModel : BaseEntity
{
    public int battery_size_wh { get; set; }

    public float rolling_resistance { get; set; }

    public float drag_coefficient { get; set; }

    public float frontal_size { get; set; }

    public int weight_kg { get; set; }

    public float avg_consumption_wh_km { get; set; }

    public string name { get; set; }

    public int ac_power { get; set; }

    public int power { get; set; }

    public int producer_id { get; set; }

    public int year { get; set; }

    public float pt_effeciency { get; set; }
}
