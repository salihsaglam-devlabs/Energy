namespace Energy.Shared.Models.V1.Inventory.StockDocumentLine.Requests;

/// <summary>StockDocumentLine oluşturma isteği.</summary>
public class CreateStockDocumentLineRequest
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
