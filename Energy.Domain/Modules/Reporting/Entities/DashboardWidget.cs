using Energy.Domain.Common;

namespace Energy.Domain.Modules.Reporting;

/// <summary>
/// Dashboard widget tanımları
/// </summary>
public class DashboardWidget : AuditableEntity
{
    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Module</summary>
    public string Module { get; set; } = string.Empty;

    /// <summary>WidgetType</summary>
    public string WidgetType { get; set; } = string.Empty;

    /// <summary>RequiredPermissionCode</summary>
    public string? RequiredPermissionCode { get; set; }

    /// <summary>DisplayOrder</summary>
    public int DisplayOrder { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
