namespace Energy.Shared.Models.V1.Workflow.ApprovalAction.Requests;

/// <summary>ApprovalAction güncelleme isteği.</summary>
public class UpdateApprovalActionRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

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
