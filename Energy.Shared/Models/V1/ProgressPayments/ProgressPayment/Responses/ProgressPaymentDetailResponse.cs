using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Responses;

/// <summary>ProgressPayment detay görünümü.</summary>
public class ProgressPaymentDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

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
