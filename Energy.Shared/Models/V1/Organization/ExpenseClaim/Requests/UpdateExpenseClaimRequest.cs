using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Organization.ExpenseClaim.Requests;

/// <summary>ExpenseClaim güncelleme isteği.</summary>
public class UpdateExpenseClaimRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
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
}
