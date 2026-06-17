using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Finance;

/// <summary>Tahsilat başlığı.</summary>
public class Collection : AuditableEntity
{
    public Guid PartnerId { get; set; }
    public Guid CurrencyId { get; set; }
    public Guid? FinancialAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CollectionDate { get; set; }
    public string CollectionNo { get; set; } = string.Empty;
    public ApprovalRequestStatus Status { get; set; } = ApprovalRequestStatus.Draft;
    public Guid? ApprovalRequestId { get; set; }
}
