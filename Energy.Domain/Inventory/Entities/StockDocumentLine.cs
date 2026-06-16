using Energy.Domain.Common;

namespace Energy.Domain.Inventory;

/// <summary>Stok belge satırı.</summary>
public class StockDocumentLine : AuditableEntity
{
    public Guid StockDocumentId { get; set; }
    public Guid MaterialId { get; set; }
    public Guid UnitOfMeasureId { get; set; }
    public decimal Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? Note { get; set; }
}
