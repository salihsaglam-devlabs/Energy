using Energy.Domain.Common;

namespace Energy.Domain.Modules.Requests;

/// <summary>
/// Talep türleri
/// </summary>
public class RequestType : AuditableEntity
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
