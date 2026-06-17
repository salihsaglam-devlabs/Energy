namespace Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Responses;

/// <summary>MaterialAttributeValue detay görünümü.</summary>
public class MaterialAttributeValueDetailResponse
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

    /// <summary>Malzeme</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Öznitelik</summary>
    public Guid MaterialAttributeDefinitionId { get; set; }

    /// <summary>Seçimli değer</summary>
    public Guid? OptionId { get; set; }

    /// <summary>Metin değer</summary>
    public string? ValueText { get; set; }

    /// <summary>Sayısal değer</summary>
    public decimal? ValueNumber { get; set; }

    /// <summary>ValueBoolean</summary>
    public bool? ValueBoolean { get; set; }

    /// <summary>ValueDate</summary>
    public DateTime? ValueDate { get; set; }
}
