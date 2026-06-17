namespace Energy.Shared.Models.V1.Catalog.Brand.Requests;

/// <summary>Brand oluşturma isteği.</summary>
public class CreateBrandRequest
{
    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
