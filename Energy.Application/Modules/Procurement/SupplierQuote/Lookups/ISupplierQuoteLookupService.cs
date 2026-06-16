using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Responses;

namespace Energy.Application.Modules.Procurement.SupplierQuote.Lookups;

/// <summary>SupplierQuote lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface ISupplierQuoteLookupService
{
    /// <summary>SupplierQuote lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<SupplierQuoteLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
