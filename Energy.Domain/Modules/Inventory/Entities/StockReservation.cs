using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>Stok rezervasyonu.</summary>
public class StockReservation : AuditableEntity
{
    public Guid WarehouseId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal Quantity { get; set; }
    public string? RelatedModule { get; set; }
    public string? RelatedEntityType { get; set; }
    public Guid? RelatedEntityId { get; set; }
    public bool IsReleased { get; set; }
}
