using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

public class Producer : BaseEntity
{
    [ColumnName("name")]
    public string Name { get; set; }
}
