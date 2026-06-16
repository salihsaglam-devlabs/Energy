namespace Energy.Shared.Models.V1.Inventory.StockBalance.Responses;

/// <summary>StockBalance liste satırı.</summary>
public class StockBalanceListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>WarehouseId</summary>
    public Guid WarehouseId { get; set; }

    /// <summary>MaterialId</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>ReservedQuantity</summary>
    public decimal ReservedQuantity { get; set; }

    /// <summary>TotalCost</summary>
    public decimal TotalCost { get; set; }

    /// <summary>LastRecalculatedAt</summary>
    public DateTime LastRecalculatedAt { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
