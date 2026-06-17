using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.ApiEndpoint.Responses;

namespace Energy.Application.IAM.ApiEndpoint.Lookups;

/// <summary>ApiEndpoint lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IApiEndpointLookupService
{
    /// <summary>ApiEndpoint lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ApiEndpointLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
