using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Workflow.ApprovalAction.Requests;

/// <summary>ApprovalAction oluşturma isteği.</summary>
public class CreateApprovalActionRequest
{
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
}
