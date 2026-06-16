using Energy.Domain.Common;

namespace Energy.Domain.Modules.Core;

/// <summary>
/// Şube tanımları
/// </summary>
public class Branch : AuditableEntity
{
    /// <summary>Şirket</summary>
    public Guid CompanyId { get; set; }

    /// <summary>Şube kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Şube adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Address</summary>
    public string? Address { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
