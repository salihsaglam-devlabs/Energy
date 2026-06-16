using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Responses;

namespace Energy.Application.Modules.Procurement.SupplierInvoice.Lookups;

/// <summary>SupplierInvoice lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface ISupplierInvoiceLookupService
{
    /// <summary>SupplierInvoice lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<SupplierInvoiceLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
