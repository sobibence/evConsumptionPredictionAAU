using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;


[TableName("edge_info")]
public class EdgeInfo : BaseEntity
{
    
    [ColumnName("osm_way_id")]
    public long OsmWayId { get; set; }
    
    [ColumnName("speed_limit_kmph")]
    public int SpeedLimit { get; set; }

    private string _streetName = "";
    [ColumnName("street_name")]
    public string StreetName
    {
        get
        {
            return _streetName;
        }
        set
        {
            if (value is not null)
            {
                _streetName = value;
            }
        }
    }

    
    private string _highway = "";
    [ColumnName("highway")]
    [EnumType]
    public string Highway
    {
        get
        {
            return _highway;
        }
        set
        {
            if (value is not null)
            {
                _highway = value;
            }
        }
    }

    private string _surface = "";
    [ColumnName("surface")]
    [EnumType]
    public string Surface
    {
        get
        {
            return _surface;
        }
        set
        {
            if (value is not null)
            {
                _surface = value;
            }
        }
    }

    public override string ToString()
    {
        return $"Way Info:\n" +
               $"OsmWayId: {OsmWayId}\n" +
               $"Speed Limit: {SpeedLimit}\n" +
               $"Street Name: {StreetName}\n" +
               $"Highway: {Highway}\n" +
               $"Surface: {Surface}";
    }
}
