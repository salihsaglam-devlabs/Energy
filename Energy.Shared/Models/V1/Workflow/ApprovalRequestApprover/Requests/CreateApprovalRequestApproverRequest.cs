namespace Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Requests;

/// <summary>ApprovalRequestApprover oluşturma isteği.</summary>
public class CreateApprovalRequestApproverRequest
{
    /// <summary>Talep adımı</summary>
    public Guid ApprovalRequestStepId { get; set; }

    /// <summary>Gerçek onaycı</summary>
    public Guid UserId { get; set; }

    /// <summary>Kişisel onay durumu</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>İşlem zamanı</summary>
    public DateTime? ActionAt { get; set; }

    /// <summary>DelegatedFromUserId</summary>
    public Guid? DelegatedFromUserId { get; set; }
}
