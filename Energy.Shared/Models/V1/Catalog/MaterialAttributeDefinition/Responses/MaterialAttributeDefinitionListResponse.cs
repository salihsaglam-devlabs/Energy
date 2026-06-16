namespace Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Responses;

/// <summary>MaterialAttributeDefinition liste satırı.</summary>
public class MaterialAttributeDefinitionListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>DataType</summary>
    public string DataType { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
