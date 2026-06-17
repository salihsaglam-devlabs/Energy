using Energy.Application.HR.Timesheet.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Timesheet.Responses;
using MediatR;

namespace Energy.Application.HR.Timesheet.Queries.GetTimesheetById;

/// <summary>
/// <see cref="GetTimesheetByIdQuery"/> handler'ı. <see cref="ITimesheetService"/>'i orkestre eder.
/// </summary>
public sealed class GetTimesheetByIdQueryHandler
    : IRequestHandler<GetTimesheetByIdQuery, BaseResponse<TimesheetDetailResponse>>
{
    private readonly ITimesheetService _service;

    public GetTimesheetByIdQueryHandler(ITimesheetService service)
        => _service = service;

    public Task<BaseResponse<TimesheetDetailResponse>> Handle(
        GetTimesheetByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
