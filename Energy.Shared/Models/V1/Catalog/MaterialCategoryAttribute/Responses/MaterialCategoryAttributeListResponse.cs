namespace Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Responses;

/// <summary>MaterialCategoryAttribute liste satırı.</summary>
public class MaterialCategoryAttributeListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>MaterialCategories referansı</summary>
    public Guid MaterialCategoryId { get; set; }

    /// <summary>MaterialAttributeDefinitions referansı</summary>
    public Guid MaterialAttributeDefinitionId { get; set; }

    /// <summary>IsRequired</summary>
    public bool IsRequired { get; set; }

    /// <summary>DisplayOrder</summary>
    public int DisplayOrder { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
