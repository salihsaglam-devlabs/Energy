using Energy.Domain.Common;

namespace Energy.Domain.Modules.Reporting;

/// <summary>
/// Rapor tanımları
/// </summary>
public class ReportDefinition : AuditableEntity
{
    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Module</summary>
    public string Module { get; set; } = string.Empty;

    /// <summary>QueryKey</summary>
    public string QueryKey { get; set; } = string.Empty;

    /// <summary>RequiredPermissionCode</summary>
    public string? RequiredPermissionCode { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
