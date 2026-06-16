using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestLine.Responses;

namespace Energy.Application.Requests.RequestLine.Lookups;

/// <summary>RequestLine lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IRequestLineLookupService
{
    /// <summary>RequestLine lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<RequestLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
