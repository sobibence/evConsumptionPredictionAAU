using EVCP.Domain.Helpers;

namespace EVCP.Domain.Models;

public class BaseEntity
{
    [OnInsertIgnore]
    [ColumnName("id")]
    public int Id { get; set; }
}
