using Energy.Domain.Common;

namespace Energy.Domain.Modules.Organization;

/// <summary>
/// Personel masraf talepleri
/// </summary>
public class ExpenseClaim : AuditableEntity
{
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
    public string Status { get; set; } = string.Empty;

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }
}
