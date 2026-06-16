using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Responses;

namespace Energy.Application.Finance.FinancialAccount.Lookups;

/// <summary>FinancialAccount lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IFinancialAccountLookupService
{
    /// <summary>FinancialAccount lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<FinancialAccountLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
