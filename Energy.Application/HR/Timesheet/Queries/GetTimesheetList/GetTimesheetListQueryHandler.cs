using Energy.Application.HR.Timesheet.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Timesheet.Responses;
using MediatR;

namespace Energy.Application.HR.Timesheet.Queries.GetTimesheetList;

/// <summary>
/// <see cref="GetTimesheetListQuery"/> handler'ı. <see cref="ITimesheetService"/>'i orkestre eder.
/// </summary>
public sealed class GetTimesheetListQueryHandler
    : IRequestHandler<GetTimesheetListQuery, BaseResponse<PaginatedResponse<TimesheetListResponse>>>
{
    private readonly ITimesheetService _service;

    public GetTimesheetListQueryHandler(ITimesheetService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<TimesheetListResponse>>> Handle(
        GetTimesheetListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
