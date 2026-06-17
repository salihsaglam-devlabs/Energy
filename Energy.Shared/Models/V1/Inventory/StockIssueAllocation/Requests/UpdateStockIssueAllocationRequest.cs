namespace Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Requests;

/// <summary>StockIssueAllocation güncelleme isteği.</summary>
public class UpdateStockIssueAllocationRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Çıkış satırı</summary>
    public Guid StockDocumentLineId { get; set; }

    /// <summary>Lot</summary>
    public Guid StockLotId { get; set; }

    /// <summary>Dağıtılan miktar</summary>
    public decimal Quantity { get; set; }

    /// <summary>Maliyet</summary>
    public decimal UnitCost { get; set; }
}
