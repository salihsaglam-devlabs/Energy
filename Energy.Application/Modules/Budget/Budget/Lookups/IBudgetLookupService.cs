using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.Budget.Responses;

namespace Energy.Application.Modules.Budget.Budget.Lookups;

/// <summary>Budget lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IBudgetLookupService
{
    /// <summary>Budget lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<BudgetLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
