using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Organization.ExpenseClaim.Responses;

/// <summary>ExpenseClaim liste satırı.</summary>
public class ExpenseClaimListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>EmployeeId</summary>
    public Guid EmployeeId { get; set; }

    /// <summary>ProjectId</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>ClaimNo</summary>
    public string ClaimNo { get; set; } = string.Empty;

    /// <summary>ClaimDate</summary>
    public DateTime ClaimDate { get; set; }

    /// <summary>TotalAmount</summary>
    public decimal TotalAmount { get; set; }

    /// <summary>Status</summary>
    public ApprovalRequestStatus Status { get; set; }

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
