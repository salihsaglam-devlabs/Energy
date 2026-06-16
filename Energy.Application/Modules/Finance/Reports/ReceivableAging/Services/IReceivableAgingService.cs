using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Reports.ReceivableAging.Requests;
using Energy.Shared.Models.V1.Finance.Reports.ReceivableAging.Responses;

namespace Energy.Application.Modules.Finance.Reports.ReceivableAging.Services;

/// <summary>ReceivableAging raporu servis sözleşmesi (salt-okunur).</summary>
public interface IReceivableAgingService
{
    /// <summary>Filtrelenmiş, sayfalanmış rapor verisini döndürür.</summary>
    Task<BaseResponse<PaginatedResponse<ReceivableAgingRowResponse>>> GetDataAsync(ReceivableAgingRequest request, CancellationToken ct = default);
}
