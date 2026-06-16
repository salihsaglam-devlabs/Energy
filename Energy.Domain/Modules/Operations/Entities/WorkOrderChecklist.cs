using Energy.Domain.Common;

namespace Energy.Domain.Modules.Operations;

/// <summary>İş emri kontrol listesi.</summary>
public class WorkOrderChecklist : AuditableEntity
{
    public Guid WorkOrderId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsRequired { get; set; } = true;
}
