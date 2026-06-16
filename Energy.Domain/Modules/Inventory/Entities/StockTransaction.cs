using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>Değiştirilemez stok hareketi.</summary>
public class StockTransaction : AuditableEntity
{
    public Guid StockDocumentId { get; set; }
    public Guid StockDocumentLineId { get; set; }
    public Guid? StockLotId { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid MaterialId { get; set; }
    /// <summary>İşaretli miktar: giriş (+), çıkış (-).</summary>
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public DateTime TransactionDate { get; set; }
}
