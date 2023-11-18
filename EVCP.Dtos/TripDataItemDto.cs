namespace EVCP.Dtos
{
    public class TripDataItemDto : ITripDataItemDto
    {
        //public Weather Weather { get; set; }
        //public Edge Edge { get; set; }

        public float EdgePercent { get; set; }

        public int TripId { get; set; }

        public float Speed { get; set; }

        public float Acceleration { get; set; }

        public DateTime Time { get; set; }

        public TripDataItemDto(float edgePercent, int tripId, float speed, float acceleration, DateTime time)
        {
            EdgePercent = edgePercent;
            TripId = tripId;
            Speed = speed;
            Acceleration = acceleration;
            Time = time;
        }
    }

    public interface ITripDataItemDto
    {
        public float EdgePercent { get; set; }

        public int TripId { get; set; }

        public float Speed { get; set; }

        public float Acceleration { get; set; }

        public DateTime Time { get; set; }
    }
}
