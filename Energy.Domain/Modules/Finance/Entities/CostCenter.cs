using Energy.Domain.Common;

namespace Energy.Domain.Modules.Finance;

/// <summary>
/// Maliyet merkezleri
/// </summary>
public class CostCenter : AuditableEntity
{
    /// <summary>ParentCostCenterId</summary>
    public Guid? ParentCostCenterId { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
