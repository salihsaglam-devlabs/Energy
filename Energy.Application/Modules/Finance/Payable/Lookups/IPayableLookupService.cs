using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payable.Responses;

namespace Energy.Application.Modules.Finance.Payable.Lookups;

/// <summary>Payable lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IPayableLookupService
{
    /// <summary>Payable lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<PayableLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
