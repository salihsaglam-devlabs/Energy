namespace Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Responses;

/// <summary>SupplierQuoteLine liste satırı.</summary>
public class SupplierQuoteLineListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>SupplierQuotes referansı</summary>
    public Guid SupplierQuoteId { get; set; }

    /// <summary>RequestLines referansı</summary>
    public Guid? RequestLineId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid? MaterialId { get; set; }

    /// <summary>Description</summary>
    public string? Description { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>UnitPrice</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>TaxRate</summary>
    public decimal TaxRate { get; set; }

    /// <summary>DiscountRate</summary>
    public decimal DiscountRate { get; set; }

    /// <summary>DeliveryDays</summary>
    public int DeliveryDays { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
