using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Reports.PayableAging.Requests;
using Energy.Shared.Models.V1.Finance.Reports.PayableAging.Responses;

namespace Energy.Application.Modules.Finance.Reports.PayableAging.Services;

/// <summary>PayableAging raporu servis sözleşmesi (salt-okunur).</summary>
public interface IPayableAgingService
{
    /// <summary>Filtrelenmiş, sayfalanmış rapor verisini döndürür.</summary>
    Task<BaseResponse<PaginatedResponse<PayableAgingRowResponse>>> GetDataAsync(PayableAgingRequest request, CancellationToken ct = default);
}
