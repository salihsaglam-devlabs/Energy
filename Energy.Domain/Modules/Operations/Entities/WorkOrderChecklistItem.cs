using Energy.Domain.Common;

namespace Energy.Domain.Modules.Operations;

/// <summary>Kontrol listesi satırı.</summary>
public class WorkOrderChecklistItem : AuditableEntity
{
    public Guid WorkOrderChecklistId { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsRequired { get; set; } = true;
    public bool IsCompleted { get; set; }
}
