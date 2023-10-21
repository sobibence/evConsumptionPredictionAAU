using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

public class BaseEntity
{
    [OnInsertIgnore]
    public int Id { get; set; }
}
