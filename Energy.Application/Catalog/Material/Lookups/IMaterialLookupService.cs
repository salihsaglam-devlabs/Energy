using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Material.Responses;

namespace Energy.Application.Catalog.Material.Lookups;

/// <summary>Material lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IMaterialLookupService
{
    /// <summary>Material lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<MaterialLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
