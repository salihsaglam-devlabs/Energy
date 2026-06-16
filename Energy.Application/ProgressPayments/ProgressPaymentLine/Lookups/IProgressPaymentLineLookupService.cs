using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Responses;

namespace Energy.Application.ProgressPayments.ProgressPaymentLine.Lookups;

/// <summary>ProgressPaymentLine lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IProgressPaymentLineLookupService
{
    /// <summary>ProgressPaymentLine lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ProgressPaymentLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
