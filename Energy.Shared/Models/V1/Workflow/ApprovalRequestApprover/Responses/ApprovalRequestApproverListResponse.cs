using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Responses;

/// <summary>ApprovalRequestApprover liste satırı.</summary>
public class ApprovalRequestApproverListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Talep adımı</summary>
    public Guid ApprovalRequestStepId { get; set; }

    /// <summary>Gerçek onaycı</summary>
    public Guid UserId { get; set; }

    /// <summary>Kişisel onay durumu</summary>
    public ApprovalApproverStatus Status { get; set; }

    /// <summary>İşlem zamanı</summary>
    public DateTime? ActionAt { get; set; }

    /// <summary>DelegatedFromUserId</summary>
    public Guid? DelegatedFromUserId { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
