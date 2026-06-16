using Energy.Domain.Common;

namespace Energy.Domain.Modules.Operations;

/// <summary>
/// İş emri durum geçmişi
/// </summary>
public class WorkOrderStatusHistory : AuditableEntity
{
    /// <summary>WorkOrderId</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>FromStatus</summary>
    public string FromStatus { get; set; } = string.Empty;

    /// <summary>ToStatus</summary>
    public string ToStatus { get; set; } = string.Empty;

    /// <summary>ChangedAt</summary>
    public DateTime ChangedAt { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }
}
