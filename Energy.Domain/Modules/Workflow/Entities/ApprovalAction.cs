using Energy.Domain.Common;

namespace Energy.Domain.Modules.Workflow;

/// <summary>
/// Onay, ret, iade ve iptal hareketleri
/// </summary>
public class ApprovalAction : AuditableEntity
{
    /// <summary>Onay talebi</summary>
    public Guid ApprovalRequestId { get; set; }

    /// <summary>Opsiyonel adım</summary>
    public Guid? ApprovalRequestStepId { get; set; }

    /// <summary>İşlem yapan</summary>
    public Guid UserId { get; set; }

    /// <summary>Approve, Reject, Return, Cancel</summary>
    public string ActionType { get; set; } = string.Empty;

    /// <summary>İşlem zamanı</summary>
    public DateTime ActionAt { get; set; }

    /// <summary>Açıklama</summary>
    public string? Note { get; set; }
}
