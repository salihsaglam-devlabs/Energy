using Energy.Domain.Common;

namespace Energy.Domain.Modules.Reporting;

/// <summary>Rapor tanımı.</summary>
public class ReportDefinition : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    /// <summary>Rapor veri kaynağı / sorgu anahtarı.</summary>
    public string QueryKey { get; set; } = string.Empty;
    public string? RequiredPermissionCode { get; set; }
    public bool IsActive { get; set; } = true;
}
