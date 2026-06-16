using Energy.Domain.Common;

namespace Energy.Domain.Modules.Operations;

/// <summary>
/// Kontrol listesi satırları
/// </summary>
public class WorkOrderChecklistItem : AuditableEntity
{
    /// <summary>WorkOrderChecklistId</summary>
    public Guid WorkOrderChecklistId { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>IsRequired</summary>
    public bool IsRequired { get; set; }

    /// <summary>IsCompleted</summary>
    public bool IsCompleted { get; set; }
}
