using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Receivable.Responses;

namespace Energy.Application.Finance.Receivable.Lookups;

/// <summary>Receivable lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IReceivableLookupService
{
    /// <summary>Receivable lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ReceivableLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
