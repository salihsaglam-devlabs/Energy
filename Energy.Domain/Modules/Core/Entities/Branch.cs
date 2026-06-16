using Energy.Domain.Common;

namespace Energy.Domain.Modules.Core;

/// <summary>Şube. <see cref="Company"/>'ye bağlıdır.</summary>
public class Branch : AuditableEntity
{
    public Guid CompanyId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
}
