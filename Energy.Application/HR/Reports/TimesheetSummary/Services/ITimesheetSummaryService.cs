using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Reports.TimesheetSummary.Requests;
using Energy.Shared.Models.V1.HR.Reports.TimesheetSummary.Responses;

namespace Energy.Application.HR.Reports.TimesheetSummary.Services;

/// <summary>TimesheetSummary raporu servis sözleşmesi (salt-okunur).</summary>
public interface ITimesheetSummaryService
{
    /// <summary>Filtrelenmiş, sayfalanmış rapor verisini döndürür.</summary>
    Task<BaseResponse<PaginatedResponse<TimesheetSummaryRowResponse>>> GetDataAsync(TimesheetSummaryRequest request, CancellationToken ct = default);
}
