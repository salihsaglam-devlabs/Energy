using Energy.Application.Modules.HR.Timesheet.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.HR.Timesheet.Commands.DeleteTimesheet;

/// <summary>
/// <see cref="DeleteTimesheetCommand"/> handler'ı. <see cref="ITimesheetService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteTimesheetCommandHandler
    : IRequestHandler<DeleteTimesheetCommand, BaseResponse<bool>>
{
    private readonly ITimesheetService _service;

    public DeleteTimesheetCommandHandler(ITimesheetService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteTimesheetCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
