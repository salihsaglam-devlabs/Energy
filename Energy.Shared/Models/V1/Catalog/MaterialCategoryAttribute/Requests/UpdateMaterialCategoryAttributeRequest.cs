namespace Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Requests;

/// <summary>MaterialCategoryAttribute güncelleme isteği.</summary>
public class UpdateMaterialCategoryAttributeRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>MaterialCategories referansı</summary>
    public Guid MaterialCategoryId { get; set; }

    /// <summary>MaterialAttributeDefinitions referansı</summary>
    public Guid MaterialAttributeDefinitionId { get; set; }

    /// <summary>IsRequired</summary>
    public bool IsRequired { get; set; }

    /// <summary>DisplayOrder</summary>
    public int DisplayOrder { get; set; }
}
