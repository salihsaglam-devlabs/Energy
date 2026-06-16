using Energy.Domain.Common;

namespace Energy.Domain.Modules.Organization;

/// <summary>Pozisyon tanımı (master/lookup).</summary>
public class EmployeePosition : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
