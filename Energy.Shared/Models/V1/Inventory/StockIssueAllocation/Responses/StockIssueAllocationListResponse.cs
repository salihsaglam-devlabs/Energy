namespace Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Responses;

/// <summary>StockIssueAllocation liste satırı.</summary>
public class StockIssueAllocationListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Çıkış satırı</summary>
    public Guid StockDocumentLineId { get; set; }

    /// <summary>Lot</summary>
    public Guid StockLotId { get; set; }

    /// <summary>Dağıtılan miktar</summary>
    public decimal Quantity { get; set; }

    /// <summary>Maliyet</summary>
    public decimal UnitCost { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
