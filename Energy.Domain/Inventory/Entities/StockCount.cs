using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Inventory;

/// <summary>Depo sayım başlığı.</summary>
public class StockCount : AuditableEntity
{
    public Guid WarehouseId { get; set; }
    public string CountNo { get; set; } = string.Empty;
    public DateTime CountDate { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
}
