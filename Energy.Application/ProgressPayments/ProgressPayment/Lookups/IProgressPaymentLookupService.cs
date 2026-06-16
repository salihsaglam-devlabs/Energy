using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Responses;

namespace Energy.Application.ProgressPayments.ProgressPayment.Lookups;

/// <summary>ProgressPayment lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IProgressPaymentLookupService
{
    /// <summary>ProgressPayment lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ProgressPaymentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
