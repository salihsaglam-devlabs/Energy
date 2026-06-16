using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>
/// Depo sayım satırları
/// </summary>
public class StockCountLine : AuditableEntity
{
    /// <summary>StockCounts referansı</summary>
    public Guid StockCountId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>SystemQuantity</summary>
    public decimal SystemQuantity { get; set; }

    /// <summary>CountedQuantity</summary>
    public decimal CountedQuantity { get; set; }
}
