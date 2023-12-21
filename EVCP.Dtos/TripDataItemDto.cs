namespace EVCP.Dtos
{
    public class TripDataItemDto : ITripDataItemDto
    {
        public float EdgePercent { get; set; }
        public int TripId { get; set; }
        public float Speed { get; set; }
        public float Acceleration { get; set; }
        public float EnergyConsumption { get; set; }
        public int VehicleId { get; set; }
        public DateTime Time { get; set; }
        public WeatherDto Weather { get; set; }
        public EdgeDto Edge { get; set; }

        public TripDataItemDto(float edgePercent, int tripId, float speed, float acceleration, DateTime time,
             float energyConsumption, int vehicleId, WeatherDto weather, EdgeDto edge)
        {
            EdgePercent = edgePercent;
            TripId = tripId;
            Speed = speed;
            Acceleration = acceleration;
            Time = time;
            Weather = weather;
            Edge = edge;
            EnergyConsumption = energyConsumption;
            VehicleId = vehicleId;
        }
    }

    public interface ITripDataItemDto
    {
        public float EdgePercent { get; set; }
        public int TripId { get; set; }
        public float Speed { get; set; }
        public float Acceleration { get; set; }
        public float EnergyConsumption { get; set; }
        public int VehicleId { get; set; }
        public DateTime Time { get; set; }
        public WeatherDto Weather { get; set; }
        public EdgeDto Edge { get; set; }
    }
}
