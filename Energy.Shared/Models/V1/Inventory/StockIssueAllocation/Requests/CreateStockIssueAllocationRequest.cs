namespace Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Requests;

/// <summary>StockIssueAllocation oluşturma isteği.</summary>
public class CreateStockIssueAllocationRequest
{
    /// <summary>Çıkış satırı</summary>
    public Guid StockDocumentLineId { get; set; }

    /// <summary>Lot</summary>
    public Guid StockLotId { get; set; }

    /// <summary>Dağıtılan miktar</summary>
    public decimal Quantity { get; set; }

    /// <summary>Maliyet</summary>
    public decimal UnitCost { get; set; }
}
