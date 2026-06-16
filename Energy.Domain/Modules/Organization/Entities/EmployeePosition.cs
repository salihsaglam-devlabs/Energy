using Energy.Domain.Common;

namespace Energy.Domain.Modules.Organization;

/// <summary>
/// Pozisyon tanımları
/// </summary>
public class EmployeePosition : AuditableEntity
{
    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
