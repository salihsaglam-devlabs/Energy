using Energy.Application.HR.TimesheetLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.TimesheetLine.Responses;
using MediatR;

namespace Energy.Application.HR.TimesheetLine.Queries.GetTimesheetLineById;

/// <summary>
/// <see cref="GetTimesheetLineByIdQuery"/> handler'ı. <see cref="ITimesheetLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetTimesheetLineByIdQueryHandler
    : IRequestHandler<GetTimesheetLineByIdQuery, BaseResponse<TimesheetLineDetailResponse>>
{
    private readonly ITimesheetLineService _service;

    public GetTimesheetLineByIdQueryHandler(ITimesheetLineService service)
        => _service = service;

    public Task<BaseResponse<TimesheetLineDetailResponse>> Handle(
        GetTimesheetLineByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
