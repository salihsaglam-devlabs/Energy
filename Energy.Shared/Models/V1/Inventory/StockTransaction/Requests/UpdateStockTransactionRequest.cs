namespace Energy.Shared.Models.V1.Inventory.StockTransaction.Requests;

/// <summary>StockTransaction güncelleme isteği.</summary>
public class UpdateStockTransactionRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>StockDocuments referansı</summary>
    public Guid StockDocumentId { get; set; }

    /// <summary>StockDocumentLines referansı</summary>
    public Guid StockDocumentLineId { get; set; }

    /// <summary>StockLots referansı</summary>
    public Guid? StockLotId { get; set; }

    /// <summary>Warehouses referansı</summary>
    public Guid WarehouseId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>UnitCost</summary>
    public decimal UnitCost { get; set; }

    /// <summary>TransactionDate</summary>
    public DateTime TransactionDate { get; set; }
}
