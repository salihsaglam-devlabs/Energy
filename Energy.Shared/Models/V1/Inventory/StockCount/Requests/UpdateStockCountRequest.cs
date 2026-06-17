using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Inventory.StockCount.Requests;

/// <summary>StockCount güncelleme isteği.</summary>
public class UpdateStockCountRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>WarehouseId</summary>
    public Guid WarehouseId { get; set; }

    /// <summary>CountNo</summary>
    public string CountNo { get; set; } = string.Empty;

    /// <summary>CountDate</summary>
    public DateTime CountDate { get; set; }

    /// <summary>Status</summary>
    public DocumentStatus Status { get; set; }
}
