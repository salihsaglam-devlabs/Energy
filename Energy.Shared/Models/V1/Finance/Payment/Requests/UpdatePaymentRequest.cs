using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Finance.Payment.Requests;

/// <summary>Payment güncelleme isteği.</summary>
public class UpdatePaymentRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

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
    public ApprovalRequestStatus Status { get; set; }

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }
}
