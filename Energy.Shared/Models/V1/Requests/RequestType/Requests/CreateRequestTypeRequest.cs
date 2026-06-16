namespace Energy.Shared.Models.V1.Requests.RequestType.Requests;

/// <summary>RequestType oluşturma isteği.</summary>
public class CreateRequestTypeRequest
{
    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Category</summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
