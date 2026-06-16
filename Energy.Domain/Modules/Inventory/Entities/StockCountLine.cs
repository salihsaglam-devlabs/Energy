using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>Depo sayım satırı.</summary>
public class StockCountLine : AuditableEntity
{
    public Guid StockCountId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal SystemQuantity { get; set; }
    public decimal CountedQuantity { get; set; }
}
