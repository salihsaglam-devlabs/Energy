using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>Stok hareket belgesi.</summary>
public class StockDocument : AuditableEntity
{
    public Guid DocumentTypeId { get; set; }
    public Guid? SourceWarehouseId { get; set; }
    public Guid? TargetWarehouseId { get; set; }
    public Guid? ProjectId { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
    public string DocumentNo { get; set; } = string.Empty;
    public DateTime DocumentDate { get; set; }
    public string? Note { get; set; }
    public Guid? ApprovalRequestId { get; set; }
}
