using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Operations;

/// <summary>İş emri durum geçmişi.</summary>
public class WorkOrderStatusHistory : AuditableEntity
{
    public Guid WorkOrderId { get; set; }
    public WorkOrderStatus FromStatus { get; set; }
    public WorkOrderStatus ToStatus { get; set; }
    public DateTime ChangedAt { get; set; }
    public string? Note { get; set; }
}
