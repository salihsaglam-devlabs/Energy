namespace Energy.Shared.Models.V1.Requests.RequestLine.Responses;

/// <summary>RequestLine detay görünümü.</summary>
public class RequestLineDetailResponse
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

    /// <summary>Talep</summary>
    public Guid RequestId { get; set; }

    /// <summary>Opsiyonel katalog malzemesi</summary>
    public Guid? MaterialId { get; set; }

    /// <summary>Serbest malzeme açıklaması</summary>
    public string? RequestedMaterialText { get; set; }

    /// <summary>Miktar</summary>
    public decimal Quantity { get; set; }

    /// <summary>Birim</summary>
    public Guid UnitOfMeasureId { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }
}
