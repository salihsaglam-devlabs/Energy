using Energy.Domain.Common;

namespace Energy.Domain.Modules.Operations;

/// <summary>
/// İş emri türleri
/// </summary>
public class WorkOrderType : AuditableEntity
{
    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
