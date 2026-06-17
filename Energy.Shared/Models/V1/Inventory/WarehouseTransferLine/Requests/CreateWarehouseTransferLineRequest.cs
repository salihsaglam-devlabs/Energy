namespace Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Requests;

/// <summary>WarehouseTransferLine oluşturma isteği.</summary>
public class CreateWarehouseTransferLineRequest
{
    /// <summary>WarehouseTransfers referansı</summary>
    public Guid WarehouseTransferId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }
}
