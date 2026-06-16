using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>
/// Depolar arası transfer satırları
/// </summary>
public class WarehouseTransferLine : AuditableEntity
{
    /// <summary>WarehouseTransfers referansı</summary>
    public Guid WarehouseTransferId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }
}
