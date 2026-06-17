namespace Energy.Shared.Models.V1.Inventory.StockBalance.Requests;

/// <summary>StockBalance güncelleme isteği.</summary>
public class UpdateStockBalanceRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
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
}
