namespace Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Requests;

/// <summary>MaterialCategoryAttribute oluşturma isteği.</summary>
public class CreateMaterialCategoryAttributeRequest
{
    /// <summary>MaterialCategories referansı</summary>
    public Guid MaterialCategoryId { get; set; }

    /// <summary>MaterialAttributeDefinitions referansı</summary>
    public Guid MaterialAttributeDefinitionId { get; set; }

    /// <summary>IsRequired</summary>
    public bool IsRequired { get; set; }

    /// <summary>DisplayOrder</summary>
    public int DisplayOrder { get; set; }
}
