using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestType.Responses;

namespace Energy.Application.Requests.RequestType.Lookups;

/// <summary>RequestType lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IRequestTypeLookupService
{
    /// <summary>RequestType lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<RequestTypeLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
