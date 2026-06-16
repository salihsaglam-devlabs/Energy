namespace Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Requests;

/// <summary>WarehouseTransferLine güncelleme isteği.</summary>
public class UpdateWarehouseTransferLineRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>WarehouseTransfers referansı</summary>
    public Guid WarehouseTransferId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }
}
