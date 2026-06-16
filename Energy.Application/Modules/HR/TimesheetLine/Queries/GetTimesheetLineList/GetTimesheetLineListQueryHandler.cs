using Energy.Application.Modules.HR.TimesheetLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.TimesheetLine.Responses;
using MediatR;

namespace Energy.Application.Modules.HR.TimesheetLine.Queries.GetTimesheetLineList;

/// <summary>
/// <see cref="GetTimesheetLineListQuery"/> handler'ı. <see cref="ITimesheetLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetTimesheetLineListQueryHandler
    : IRequestHandler<GetTimesheetLineListQuery, BaseResponse<PaginatedResponse<TimesheetLineListResponse>>>
{
    private readonly ITimesheetLineService _service;

    public GetTimesheetLineListQueryHandler(ITimesheetLineService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<TimesheetLineListResponse>>> Handle(
        GetTimesheetLineListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
