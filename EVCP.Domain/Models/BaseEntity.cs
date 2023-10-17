using EVCP.Domain.Custom;

namespace EVCP.Domain.Models;

public class BaseEntity
{
    [OnInsertIgnore]
    public int id { get; set; }
}
