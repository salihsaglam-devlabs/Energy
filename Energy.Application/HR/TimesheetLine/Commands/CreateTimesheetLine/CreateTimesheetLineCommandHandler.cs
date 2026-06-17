using Energy.Application.HR.TimesheetLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.HR.TimesheetLine.Commands.CreateTimesheetLine;

/// <summary>
/// <see cref="CreateTimesheetLineCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="ITimesheetLineService"/>'i orkestre eder.
/// </summary>
public sealed class CreateTimesheetLineCommandHandler
    : IRequestHandler<CreateTimesheetLineCommand, BaseResponse<Guid>>
{
    private readonly ITimesheetLineService _service;

    public CreateTimesheetLineCommandHandler(ITimesheetLineService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateTimesheetLineCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
