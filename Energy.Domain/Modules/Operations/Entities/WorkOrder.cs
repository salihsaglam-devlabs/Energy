using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Modules.Operations;

/// <summary>İş emri (proje bazlı veya bağımsız).</summary>
public class WorkOrder : AuditableEntity
{
    public Guid WorkOrderTypeId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? ProjectPhaseId { get; set; }
    public Guid? ProjectLocationId { get; set; }
    public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Draft;
    public string WorkOrderNo { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? PlannedStart { get; set; }
    public DateTime? PlannedEnd { get; set; }
}
