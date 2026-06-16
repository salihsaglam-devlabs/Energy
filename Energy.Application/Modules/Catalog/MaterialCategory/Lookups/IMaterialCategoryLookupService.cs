using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Responses;

namespace Energy.Application.Modules.Catalog.MaterialCategory.Lookups;

/// <summary>MaterialCategory lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IMaterialCategoryLookupService
{
    /// <summary>MaterialCategory lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<MaterialCategoryLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
