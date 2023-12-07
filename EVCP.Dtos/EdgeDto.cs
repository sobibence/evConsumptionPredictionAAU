namespace EVCP.Dtos
{
    public class EdgeDto
    {
        public long StartNodeId { get; set; }
        public long EndNodeId { get; set; }
        public long OsmWayId { get; set; }
        public double Length { get; set; }
        public int SpeedLimit { get; set; }
        public string StreetName { get; set; }
        public string Highway { get; set; }
        public string Surface { get; set; }

        public EdgeDto(long startNodeId, long endNodeId, long osmWayId, double length, int speedLimit, string streetName, string highway, string surface)
        {
            StartNodeId = startNodeId;
            EndNodeId = endNodeId;
            OsmWayId = osmWayId;
            Length = length;
            SpeedLimit = speedLimit;
            StreetName = streetName;
            Highway = highway;
            Surface = surface;
        }
    }
}
