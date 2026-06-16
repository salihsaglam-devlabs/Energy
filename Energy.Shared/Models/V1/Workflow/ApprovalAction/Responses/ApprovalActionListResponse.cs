using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Workflow.ApprovalAction.Responses;

/// <summary>ApprovalAction liste satırı.</summary>
public class ApprovalActionListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Onay talebi</summary>
    public Guid ApprovalRequestId { get; set; }

    /// <summary>Opsiyonel adım</summary>
    public Guid? ApprovalRequestStepId { get; set; }

    /// <summary>İşlem yapan</summary>
    public Guid UserId { get; set; }

    /// <summary>Approve, Reject, Return, Cancel</summary>
    public ApprovalActionType ActionType { get; set; }

    /// <summary>İşlem zamanı</summary>
    public DateTime ActionAt { get; set; }

    /// <summary>Açıklama</summary>
    public string? Note { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
