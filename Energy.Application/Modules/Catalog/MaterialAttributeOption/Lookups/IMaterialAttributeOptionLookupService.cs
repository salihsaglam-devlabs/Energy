using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Responses;

namespace Energy.Application.Modules.Catalog.MaterialAttributeOption.Lookups;

/// <summary>MaterialAttributeOption lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IMaterialAttributeOptionLookupService
{
    /// <summary>MaterialAttributeOption lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<MaterialAttributeOptionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
