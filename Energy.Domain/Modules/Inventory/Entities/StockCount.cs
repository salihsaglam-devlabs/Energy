using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>
/// Depo sayım başlıkları
/// </summary>
public class StockCount : AuditableEntity
{
    /// <summary>WarehouseId</summary>
    public Guid WarehouseId { get; set; }

    /// <summary>CountNo</summary>
    public string CountNo { get; set; } = string.Empty;

    /// <summary>CountDate</summary>
    public DateTime CountDate { get; set; }

    /// <summary>Status</summary>
    public string Status { get; set; } = string.Empty;
}
