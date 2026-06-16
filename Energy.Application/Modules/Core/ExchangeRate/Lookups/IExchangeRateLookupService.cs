using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.ExchangeRate.Responses;

namespace Energy.Application.Modules.Core.ExchangeRate.Lookups;

/// <summary>ExchangeRate lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IExchangeRateLookupService
{
    /// <summary>ExchangeRate lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ExchangeRateLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
