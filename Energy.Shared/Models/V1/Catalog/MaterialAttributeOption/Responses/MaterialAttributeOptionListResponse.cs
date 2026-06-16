namespace Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Responses;

/// <summary>MaterialAttributeOption liste satırı.</summary>
public class MaterialAttributeOptionListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>MaterialAttributeDefinitions referansı</summary>
    public Guid MaterialAttributeDefinitionId { get; set; }

    /// <summary>Value</summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>DisplayOrder</summary>
    public int DisplayOrder { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
