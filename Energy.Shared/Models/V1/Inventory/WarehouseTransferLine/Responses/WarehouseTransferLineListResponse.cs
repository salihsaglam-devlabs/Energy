namespace Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Responses;

/// <summary>WarehouseTransferLine liste satırı.</summary>
public class WarehouseTransferLineListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>WarehouseTransfers referansı</summary>
    public Guid WarehouseTransferId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
