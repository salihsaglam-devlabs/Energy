using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Responses;

namespace Energy.Application.Modules.Finance.FinancialTransactionLine.Lookups;

/// <summary>FinancialTransactionLine lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IFinancialTransactionLineLookupService
{
    /// <summary>FinancialTransactionLine lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<FinancialTransactionLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
