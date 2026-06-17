namespace Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Responses;

/// <summary>MaterialCategoryAttribute detay görünümü.</summary>
public class MaterialCategoryAttributeDetailResponse
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

    /// <summary>MaterialCategories referansı</summary>
    public Guid MaterialCategoryId { get; set; }

    /// <summary>MaterialAttributeDefinitions referansı</summary>
    public Guid MaterialAttributeDefinitionId { get; set; }

    /// <summary>IsRequired</summary>
    public bool IsRequired { get; set; }

    /// <summary>DisplayOrder</summary>
    public int DisplayOrder { get; set; }
}
