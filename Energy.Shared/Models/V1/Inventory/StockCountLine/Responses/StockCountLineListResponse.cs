namespace Energy.Shared.Models.V1.Inventory.StockCountLine.Responses;

/// <summary>StockCountLine liste satırı.</summary>
public class StockCountLineListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>StockCounts referansı</summary>
    public Guid StockCountId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>SystemQuantity</summary>
    public decimal SystemQuantity { get; set; }

    /// <summary>CountedQuantity</summary>
    public decimal CountedQuantity { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
