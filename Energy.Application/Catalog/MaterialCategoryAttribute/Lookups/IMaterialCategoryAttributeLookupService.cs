using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Responses;

namespace Energy.Application.Catalog.MaterialCategoryAttribute.Lookups;

/// <summary>MaterialCategoryAttribute lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IMaterialCategoryAttributeLookupService
{
    /// <summary>MaterialCategoryAttribute lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<MaterialCategoryAttributeLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
