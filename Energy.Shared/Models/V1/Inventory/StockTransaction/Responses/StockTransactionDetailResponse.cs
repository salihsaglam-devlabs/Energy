namespace Energy.Shared.Models.V1.Inventory.StockTransaction.Responses;

/// <summary>StockTransaction detay görünümü.</summary>
public class StockTransactionDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

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
