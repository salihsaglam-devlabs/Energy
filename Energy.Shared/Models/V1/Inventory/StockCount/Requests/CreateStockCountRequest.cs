using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Inventory.StockCount.Requests;

/// <summary>StockCount oluşturma isteği.</summary>
public class CreateStockCountRequest
{
    /// <summary>WarehouseId</summary>
    public Guid WarehouseId { get; set; }

    /// <summary>CountNo</summary>
    public string CountNo { get; set; } = string.Empty;

    /// <summary>CountDate</summary>
    public DateTime CountDate { get; set; }

    /// <summary>Status</summary>
    public DocumentStatus Status { get; set; }
}
