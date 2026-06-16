using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>Depolar arası transfer başlığı.</summary>
public class WarehouseTransfer : AuditableEntity
{
    public Guid SourceWarehouseId { get; set; }
    public Guid TargetWarehouseId { get; set; }
    public string TransferNo { get; set; } = string.Empty;
    public DateTime TransferDate { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
}
