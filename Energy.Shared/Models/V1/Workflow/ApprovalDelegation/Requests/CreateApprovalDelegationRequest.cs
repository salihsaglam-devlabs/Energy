namespace Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Requests;

/// <summary>ApprovalDelegation oluşturma isteği.</summary>
public class CreateApprovalDelegationRequest
{
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
}
