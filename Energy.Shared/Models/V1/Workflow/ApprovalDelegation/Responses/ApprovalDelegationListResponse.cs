namespace Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Responses;

/// <summary>ApprovalDelegation liste satırı.</summary>
public class ApprovalDelegationListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>DelegatorUserId</summary>
    public Guid DelegatorUserId { get; set; }

    /// <summary>DelegateUserId</summary>
    public Guid DelegateUserId { get; set; }

    /// <summary>StartDate</summary>
    public DateTime StartDate { get; set; }

    /// <summary>EndDate</summary>
    public DateTime EndDate { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
