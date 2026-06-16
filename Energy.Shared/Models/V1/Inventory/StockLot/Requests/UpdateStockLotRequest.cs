namespace Energy.Shared.Models.V1.Inventory.StockLot.Requests;

/// <summary>StockLot güncelleme isteği.</summary>
public class UpdateStockLotRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

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
