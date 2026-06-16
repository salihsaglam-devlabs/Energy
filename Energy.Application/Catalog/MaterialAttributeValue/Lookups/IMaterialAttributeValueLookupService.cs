using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Responses;

namespace Energy.Application.Catalog.MaterialAttributeValue.Lookups;

/// <summary>MaterialAttributeValue lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IMaterialAttributeValueLookupService
{
    /// <summary>MaterialAttributeValue lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<MaterialAttributeValueLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
