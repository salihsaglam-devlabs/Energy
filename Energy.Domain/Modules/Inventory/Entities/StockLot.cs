using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>Lot ve maliyet katmanı (her giriş ayrı lot).</summary>
public class StockLot : AuditableEntity
{
    public Guid WarehouseId { get; set; }
    public Guid MaterialId { get; set; }
    public Guid SourceStockDocumentLineId { get; set; }
    public string LotNo { get; set; } = string.Empty;
    public decimal InitialQuantity { get; set; }
    public decimal RemainingQuantity { get; set; }
    public decimal UnitCost { get; set; }
    public DateTime ReceivedAt { get; set; }
}
