using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.BudgetLine.Responses;

namespace Energy.Application.Budget.BudgetLine.Lookups;

/// <summary>BudgetLine lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IBudgetLineLookupService
{
    /// <summary>BudgetLine lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<BudgetLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
