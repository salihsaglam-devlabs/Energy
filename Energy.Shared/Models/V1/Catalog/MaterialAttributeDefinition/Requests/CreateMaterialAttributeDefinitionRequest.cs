namespace Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Requests;

/// <summary>MaterialAttributeDefinition oluşturma isteği.</summary>
public class CreateMaterialAttributeDefinitionRequest
{
    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>DataType</summary>
    public string DataType { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
