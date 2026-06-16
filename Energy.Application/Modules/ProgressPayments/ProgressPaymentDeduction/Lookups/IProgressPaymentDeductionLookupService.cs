using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Responses;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Lookups;

/// <summary>ProgressPaymentDeduction lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IProgressPaymentDeductionLookupService
{
    /// <summary>ProgressPaymentDeduction lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ProgressPaymentDeductionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
