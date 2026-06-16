using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>
/// Stok belge satırları
/// </summary>
public class StockDocumentLine : AuditableEntity
{
    /// <summary>Belge</summary>
    public Guid StockDocumentId { get; set; }

    /// <summary>Malzeme</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Birim</summary>
    public Guid UnitOfMeasureId { get; set; }

    /// <summary>Miktar</summary>
    public decimal Quantity { get; set; }

    /// <summary>Birim fiyat</summary>
    public decimal? UnitPrice { get; set; }

    /// <summary>Para birimi</summary>
    public Guid? CurrencyId { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }
}
