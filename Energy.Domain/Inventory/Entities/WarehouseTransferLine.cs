using Energy.Domain.Common;

namespace Energy.Domain.Inventory;

/// <summary>Depolar arası transfer satırı.</summary>
public class WarehouseTransferLine : AuditableEntity
{
    public Guid WarehouseTransferId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal Quantity { get; set; }
}
