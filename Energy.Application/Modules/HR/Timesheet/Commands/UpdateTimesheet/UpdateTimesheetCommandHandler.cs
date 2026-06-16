using Energy.Application.Modules.HR.Timesheet.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.HR.Timesheet.Commands.UpdateTimesheet;

/// <summary>
/// <see cref="UpdateTimesheetCommand"/> handler'ı. <see cref="ITimesheetService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateTimesheetCommandHandler
    : IRequestHandler<UpdateTimesheetCommand, BaseResponse<bool>>
{
    private readonly ITimesheetService _service;

    public UpdateTimesheetCommandHandler(ITimesheetService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateTimesheetCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
