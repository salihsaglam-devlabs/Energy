using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Procurement;

/// <summary>Tedarikçi teklif başlığı.</summary>
public class SupplierQuote : AuditableEntity
{
    public Guid SupplierId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid CurrencyId { get; set; }
    public string QuoteNo { get; set; } = string.Empty;
    public DateTime QuoteDate { get; set; }
    public string? PaymentTerm { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
    public bool IsSelected { get; set; }
}
