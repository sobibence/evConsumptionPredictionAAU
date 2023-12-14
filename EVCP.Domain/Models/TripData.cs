using EVCP.Domain.Models;

namespace EVCP.Domain;

public class TripData : BaseEntity
{
    public Weather Weather { get; set; }
    public Edge Edge { get; set; }

    public float EdgePercent { get; set; }

    public int TripId { get; set; }

    public float Speed { get; set; }

    public float Acceleration { get; set; }

    public float EnergyConsumption { get; set; }

    public int VehicleId { get; set; }

    public DateTime Time { get; set; }

    public override string ToString()
    {
        return $"TripData Info:\n" +
               $"EdgePercent: {EdgePercent}\n" +
               $"TripId: {TripId}\n" +
               $"Speed: {Speed}\n" +
               $"Time: {Time}\n" +
               $"Acceleration: {Acceleration}\n" +
               $"Edge: {Edge.EdgeInfo.OsmWayId}";
    }
}