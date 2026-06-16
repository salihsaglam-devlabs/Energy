using Energy.Domain.Common;

namespace Energy.Domain.Modules.FieldOperations;

/// <summary>
/// Günlük saha raporları
/// </summary>
public class DailySiteReport : AuditableEntity
{
    /// <summary>Projects referansı</summary>
    public Guid ProjectId { get; set; }

    /// <summary>WorkOrderId</summary>
    public Guid? WorkOrderId { get; set; }

    /// <summary>ReportNo</summary>
    public string ReportNo { get; set; } = string.Empty;

    /// <summary>ReportDate</summary>
    public DateTime ReportDate { get; set; }

    /// <summary>Weather</summary>
    public string? Weather { get; set; }

    /// <summary>Notes</summary>
    public string? Notes { get; set; }

    /// <summary>Status</summary>
    public string Status { get; set; } = string.Empty;
}
