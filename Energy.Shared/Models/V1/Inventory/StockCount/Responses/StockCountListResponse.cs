using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Inventory.StockCount.Responses;

/// <summary>StockCount liste satırı.</summary>
public class StockCountListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>WarehouseId</summary>
    public Guid WarehouseId { get; set; }

    /// <summary>CountNo</summary>
    public string CountNo { get; set; } = string.Empty;

    /// <summary>CountDate</summary>
    public DateTime CountDate { get; set; }

    /// <summary>Status</summary>
    public DocumentStatus Status { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
