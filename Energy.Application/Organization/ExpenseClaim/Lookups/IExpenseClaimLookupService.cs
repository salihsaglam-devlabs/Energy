using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Responses;

namespace Energy.Application.Organization.ExpenseClaim.Lookups;

/// <summary>ExpenseClaim lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IExpenseClaimLookupService
{
    /// <summary>ExpenseClaim lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ExpenseClaimLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
