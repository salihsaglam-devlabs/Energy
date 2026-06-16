namespace Energy.Shared.Models.V1.Finance.CollectionAllocation.Responses;

/// <summary>CollectionAllocation detay görünümü.</summary>
public class CollectionAllocationDetailResponse
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

    /// <summary>CollectionId</summary>
    public Guid CollectionId { get; set; }

    /// <summary>ReceivableId</summary>
    public Guid ReceivableId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }
}
