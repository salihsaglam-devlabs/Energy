namespace Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Requests;

/// <summary>MaterialAttributeOption oluşturma isteği.</summary>
public class CreateMaterialAttributeOptionRequest
{
    /// <summary>MaterialAttributeDefinitions referansı</summary>
    public Guid MaterialAttributeDefinitionId { get; set; }

    /// <summary>Value</summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>DisplayOrder</summary>
    public int DisplayOrder { get; set; }
}
