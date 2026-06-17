using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.FieldOperations;

/// <summary>Günlük saha raporu (proje bazlı).</summary>
public class DailySiteReport : AuditableEntity
{
    public Guid ProjectId { get; set; }
    public Guid? WorkOrderId { get; set; }
    public string ReportNo { get; set; } = string.Empty;
    public DateTime ReportDate { get; set; }
    public string? Weather { get; set; }
    public string? Notes { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
}
