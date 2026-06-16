using Energy.Domain.Common;

namespace Energy.Domain.Modules.Operations;

/// <summary>İş emri türü.</summary>
public class WorkOrderType : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
