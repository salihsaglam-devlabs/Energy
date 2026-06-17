using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.LocalizationResource.Responses;

namespace Energy.Application.Core.LocalizationResource.Lookups;

/// <summary>LocalizationResource lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface ILocalizationResourceLookupService
{
    /// <summary>LocalizationResource lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<LocalizationResourceLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
