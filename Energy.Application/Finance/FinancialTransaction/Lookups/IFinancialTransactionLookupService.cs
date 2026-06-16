using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Responses;

namespace Energy.Application.Finance.FinancialTransaction.Lookups;

/// <summary>FinancialTransaction lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IFinancialTransactionLookupService
{
    /// <summary>FinancialTransaction lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<FinancialTransactionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
