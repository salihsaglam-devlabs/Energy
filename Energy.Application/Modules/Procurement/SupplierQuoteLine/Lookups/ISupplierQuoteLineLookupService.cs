using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Responses;

namespace Energy.Application.Modules.Procurement.SupplierQuoteLine.Lookups;

/// <summary>SupplierQuoteLine lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface ISupplierQuoteLineLookupService
{
    /// <summary>SupplierQuoteLine lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<SupplierQuoteLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
