namespace EVCP.Dtos
{
    public class TripDataDto : ITripDataDto
    {
        public DateTime SourceTimestamp { get; set; }
        public IEnumerable<ITripDataItemDto> Data { get; set; }

        public TripDataDto(DateTime sourceTimestamp, IEnumerable<ITripDataItemDto> data)
        {
            SourceTimestamp = sourceTimestamp;
            Data = data;
        }
    }

    public interface ITripDataDto
    {
        public DateTime SourceTimestamp { get; set; }
        public IEnumerable<ITripDataItemDto> Data { get; set; }
    }
}
