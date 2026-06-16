using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Currency.Responses;

namespace Energy.Application.Core.Currency.Lookups;

/// <summary>Currency lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface ICurrencyLookupService
{
    /// <summary>Currency lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<CurrencyLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
