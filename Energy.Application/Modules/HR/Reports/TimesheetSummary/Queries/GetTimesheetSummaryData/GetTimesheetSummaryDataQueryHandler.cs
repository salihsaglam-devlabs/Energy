using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Reports.TimesheetSummary.Requests;
using Energy.Shared.Models.V1.HR.Reports.TimesheetSummary.Responses;
using Energy.Application.Modules.HR.Reports.TimesheetSummary.Services;
using MediatR;

namespace Energy.Application.Modules.HR.Reports.TimesheetSummary.Queries.GetTimesheetSummaryData;

/// <summary><see cref="GetTimesheetSummaryDataQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetTimesheetSummaryDataQueryHandler
    : IRequestHandler<GetTimesheetSummaryDataQuery, BaseResponse<PaginatedResponse<TimesheetSummaryRowResponse>>>
{
    private readonly ITimesheetSummaryService _service;

    public GetTimesheetSummaryDataQueryHandler(ITimesheetSummaryService service)
    {
        _service = service;
    }

    public async Task<BaseResponse<PaginatedResponse<TimesheetSummaryRowResponse>>> Handle(GetTimesheetSummaryDataQuery request, CancellationToken ct)
    {
        return await _service.GetDataAsync(request.Request, ct);
    }
}
