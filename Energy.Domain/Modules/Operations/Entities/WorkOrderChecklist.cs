using Energy.Domain.Common;

namespace Energy.Domain.Modules.Operations;

/// <summary>
/// İş emri kontrol listeleri
/// </summary>
public class WorkOrderChecklist : AuditableEntity
{
    /// <summary>WorkOrderId</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsRequired</summary>
    public bool IsRequired { get; set; }
}
