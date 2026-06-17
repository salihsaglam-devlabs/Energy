using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Finance.Payment.Responses;

/// <summary>Payment detay görünümü.</summary>
public class PaymentDetailResponse
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
