using Energy.Domain.Common;

namespace Energy.Domain.Inventory;

/// <summary>Özet stok bakiyesi (hareketlerden yeniden üretilebilir).</summary>
public class StockBalance : AuditableEntity
{
    public Guid WarehouseId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal Quantity { get; set; }
    public decimal ReservedQuantity { get; set; }
    public decimal TotalCost { get; set; }
    public DateTime LastRecalculatedAt { get; set; }
}
