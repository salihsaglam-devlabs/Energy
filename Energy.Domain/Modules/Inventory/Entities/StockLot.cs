using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>
/// Lot ve maliyet katmanları
/// </summary>
public class StockLot : AuditableEntity
{
    /// <summary>Depo</summary>
    public Guid WarehouseId { get; set; }

    /// <summary>Malzeme</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Kaynak giriş satırı</summary>
    public Guid SourceStockDocumentLineId { get; set; }

    /// <summary>Lot no</summary>
    public string LotNo { get; set; } = string.Empty;

    /// <summary>İlk miktar</summary>
    public decimal InitialQuantity { get; set; }

    /// <summary>Kalan miktar</summary>
    public decimal RemainingQuantity { get; set; }

    /// <summary>Maliyet</summary>
    public decimal UnitCost { get; set; }

    /// <summary>ReceivedAt</summary>
    public DateTime ReceivedAt { get; set; }
}
