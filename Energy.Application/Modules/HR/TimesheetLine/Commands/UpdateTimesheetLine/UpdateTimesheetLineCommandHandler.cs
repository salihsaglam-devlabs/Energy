using Energy.Application.Modules.HR.TimesheetLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.HR.TimesheetLine.Commands.UpdateTimesheetLine;

/// <summary>
/// <see cref="UpdateTimesheetLineCommand"/> handler'ı. <see cref="ITimesheetLineService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateTimesheetLineCommandHandler
    : IRequestHandler<UpdateTimesheetLineCommand, BaseResponse<bool>>
{
    private readonly ITimesheetLineService _service;

    public UpdateTimesheetLineCommandHandler(ITimesheetLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateTimesheetLineCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
