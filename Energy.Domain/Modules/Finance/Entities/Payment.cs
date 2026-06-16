using Energy.Domain.Common;

namespace Energy.Domain.Modules.Finance;

/// <summary>
/// Ödeme başlıkları
/// </summary>
public class Payment : AuditableEntity
{
    /// <summary>PartnerId</summary>
    public Guid PartnerId { get; set; }

    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>FinancialAccountId</summary>
    public Guid? FinancialAccountId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }

    /// <summary>PaymentDate</summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>PaymentNo</summary>
    public string PaymentNo { get; set; } = string.Empty;

    /// <summary>Status</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }
}
