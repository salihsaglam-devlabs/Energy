using Energy.Application.HR.Timesheet.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.HR.Timesheet.Commands.CreateTimesheet;

/// <summary>
/// <see cref="CreateTimesheetCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="ITimesheetService"/>'i orkestre eder.
/// </summary>
public sealed class CreateTimesheetCommandHandler
    : IRequestHandler<CreateTimesheetCommand, BaseResponse<Guid>>
{
    private readonly ITimesheetService _service;

    public CreateTimesheetCommandHandler(ITimesheetService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateTimesheetCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
