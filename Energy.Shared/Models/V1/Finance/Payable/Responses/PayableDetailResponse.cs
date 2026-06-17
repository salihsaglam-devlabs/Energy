namespace Energy.Shared.Models.V1.Finance.Payable.Responses;

/// <summary>Payable detay görünümü.</summary>
public class PayableDetailResponse
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

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }

    /// <summary>RemainingAmount</summary>
    public decimal RemainingAmount { get; set; }

    /// <summary>DueDate</summary>
    public DateTime DueDate { get; set; }

    /// <summary>RelatedModule</summary>
    public string? RelatedModule { get; set; }

    /// <summary>RelatedEntityType</summary>
    public string? RelatedEntityType { get; set; }

    /// <summary>RelatedEntityId</summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>IsClosed</summary>
    public bool IsClosed { get; set; }
}
