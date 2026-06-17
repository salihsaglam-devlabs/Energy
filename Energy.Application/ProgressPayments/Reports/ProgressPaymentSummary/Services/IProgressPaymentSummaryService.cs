using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.Reports.ProgressPaymentSummary.Requests;
using Energy.Shared.Models.V1.ProgressPayments.Reports.ProgressPaymentSummary.Responses;

namespace Energy.Application.ProgressPayments.Reports.ProgressPaymentSummary.Services;

/// <summary>ProgressPaymentSummary raporu servis sözleşmesi (salt-okunur).</summary>
public interface IProgressPaymentSummaryService
{
    /// <summary>Filtrelenmiş, sayfalanmış rapor verisini döndürür.</summary>
    Task<BaseResponse<PaginatedResponse<ProgressPaymentSummaryRowResponse>>> GetDataAsync(ProgressPaymentSummaryRequest request, CancellationToken ct = default);
}
