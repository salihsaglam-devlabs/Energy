using Energy.Domain.Common;

namespace Energy.Domain.Modules.Reporting;

/// <summary>Dashboard widget tanımı.</summary>
public class DashboardWidget : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    /// <summary>Chart, Counter, Grid, Gauge vb.</summary>
    public string WidgetType { get; set; } = "Counter";
    public string? RequiredPermissionCode { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
