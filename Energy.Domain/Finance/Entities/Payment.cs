using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Finance;

/// <summary>Ödeme başlığı.</summary>
public class Payment : AuditableEntity
{
    public Guid PartnerId { get; set; }
    public Guid CurrencyId { get; set; }
    public Guid? FinancialAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentNo { get; set; } = string.Empty;
    public ApprovalRequestStatus Status { get; set; } = ApprovalRequestStatus.Draft;
    public Guid? ApprovalRequestId { get; set; }
}
