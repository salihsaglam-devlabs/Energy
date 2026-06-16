using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Responses;

namespace Energy.Application.Modules.Procurement.SupplierInvoiceLine.Lookups;

/// <summary>SupplierInvoiceLine lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface ISupplierInvoiceLineLookupService
{
    /// <summary>SupplierInvoiceLine lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<SupplierInvoiceLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
