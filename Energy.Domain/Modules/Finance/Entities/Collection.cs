using Energy.Domain.Common;

namespace Energy.Domain.Modules.Finance;

/// <summary>
/// Tahsilat başlıkları
/// </summary>
public class Collection : AuditableEntity
{
    /// <summary>PartnerId</summary>
    public Guid PartnerId { get; set; }

    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>FinancialAccountId</summary>
    public Guid? FinancialAccountId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }

    /// <summary>CollectionDate</summary>
    public DateTime CollectionDate { get; set; }

    /// <summary>CollectionNo</summary>
    public string CollectionNo { get; set; } = string.Empty;

    /// <summary>Status</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }
}
