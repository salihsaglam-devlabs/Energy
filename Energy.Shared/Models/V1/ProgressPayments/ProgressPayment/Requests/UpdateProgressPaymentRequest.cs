using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Requests;

/// <summary>ProgressPayment güncelleme isteği.</summary>
public class UpdateProgressPaymentRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>ContractId</summary>
    public Guid ContractId { get; set; }

    /// <summary>PartnerId</summary>
    public Guid? PartnerId { get; set; }

    /// <summary>ProgressPaymentNo</summary>
    public string ProgressPaymentNo { get; set; } = string.Empty;

    /// <summary>PaymentPeriodStart</summary>
    public DateTime PaymentPeriodStart { get; set; }

    /// <summary>PaymentPeriodEnd</summary>
    public DateTime PaymentPeriodEnd { get; set; }

    /// <summary>GrossAmount</summary>
    public decimal GrossAmount { get; set; }

    /// <summary>DeductionTotal</summary>
    public decimal DeductionTotal { get; set; }

    /// <summary>NetAmount</summary>
    public decimal NetAmount { get; set; }

    /// <summary>Status</summary>
    public ApprovalRequestStatus Status { get; set; }

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }
}
