using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Brand.Responses;

namespace Energy.Application.Catalog.Brand.Lookups;

/// <summary>Brand lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IBrandLookupService
{
    /// <summary>Brand lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<BrandLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
