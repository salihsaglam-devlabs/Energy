using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Responses;

namespace Energy.Application.Modules.Organization.ExpenseClaimLine.Lookups;

/// <summary>ExpenseClaimLine lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IExpenseClaimLineLookupService
{
    /// <summary>ExpenseClaimLine lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ExpenseClaimLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
