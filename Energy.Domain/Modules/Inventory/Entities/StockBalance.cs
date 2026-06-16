using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>
/// Özet stok bakiyeleri
/// </summary>
public class StockBalance : AuditableEntity
{
    /// <summary>WarehouseId</summary>
    public Guid WarehouseId { get; set; }

    /// <summary>MaterialId</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>ReservedQuantity</summary>
    public decimal ReservedQuantity { get; set; }

    /// <summary>TotalCost</summary>
    public decimal TotalCost { get; set; }

    /// <summary>LastRecalculatedAt</summary>
    public DateTime LastRecalculatedAt { get; set; }
}
