namespace Energy.Shared.Models.V1.Inventory.StockCountLine.Requests;

/// <summary>StockCountLine güncelleme isteği.</summary>
public class UpdateStockCountLineRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>StockCounts referansı</summary>
    public Guid StockCountId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>SystemQuantity</summary>
    public decimal SystemQuantity { get; set; }

    /// <summary>CountedQuantity</summary>
    public decimal CountedQuantity { get; set; }
}
