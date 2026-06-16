namespace Energy.Shared.Models.V1.Finance.PaymentAllocation.Responses;

/// <summary>PaymentAllocation detay görünümü.</summary>
public class PaymentAllocationDetailResponse
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

    /// <summary>PaymentId</summary>
    public Guid PaymentId { get; set; }

    /// <summary>PayableId</summary>
    public Guid PayableId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }
}
