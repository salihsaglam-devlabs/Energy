using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Modules.Organization;

/// <summary>Personel masraf talebi başlığı.</summary>
public class ExpenseClaim : AuditableEntity
{
    public Guid EmployeeId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid CurrencyId { get; set; }
    public string ClaimNo { get; set; } = string.Empty;
    public DateTime ClaimDate { get; set; }
    public decimal TotalAmount { get; set; }
    public ApprovalRequestStatus Status { get; set; } = ApprovalRequestStatus.Draft;
    public Guid? ApprovalRequestId { get; set; }
}
