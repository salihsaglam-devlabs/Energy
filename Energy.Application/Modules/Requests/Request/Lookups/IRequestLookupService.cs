using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.Request.Responses;

namespace Energy.Application.Modules.Requests.Request.Lookups;

/// <summary>Request lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IRequestLookupService
{
    /// <summary>Request lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<RequestLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
