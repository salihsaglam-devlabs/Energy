using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payment.Responses;

namespace Energy.Application.Modules.Finance.Payment.Lookups;

/// <summary>Payment lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IPaymentLookupService
{
    /// <summary>Payment lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<PaymentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
