using Energy.Application.HR.TimesheetLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.HR.TimesheetLine.Commands.DeleteTimesheetLine;

/// <summary>
/// <see cref="DeleteTimesheetLineCommand"/> handler'ı. <see cref="ITimesheetLineService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteTimesheetLineCommandHandler
    : IRequestHandler<DeleteTimesheetLineCommand, BaseResponse<bool>>
{
    private readonly ITimesheetLineService _service;

    public DeleteTimesheetLineCommandHandler(ITimesheetLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteTimesheetLineCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
