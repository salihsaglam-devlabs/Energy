using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Responses;

namespace Energy.Application.Finance.PaymentAllocation.Lookups;

/// <summary>PaymentAllocation lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IPaymentAllocationLookupService
{
    /// <summary>PaymentAllocation lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<PaymentAllocationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
