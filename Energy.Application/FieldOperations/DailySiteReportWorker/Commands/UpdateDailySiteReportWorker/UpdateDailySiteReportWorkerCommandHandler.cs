using Energy.Application.FieldOperations.DailySiteReportWorker.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportWorker.Commands.UpdateDailySiteReportWorker;

/// <summary>
/// <see cref="UpdateDailySiteReportWorkerCommand"/> handler'ı. <see cref="IDailySiteReportWorkerService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateDailySiteReportWorkerCommandHandler
    : IRequestHandler<UpdateDailySiteReportWorkerCommand, BaseResponse<bool>>
{
    private readonly IDailySiteReportWorkerService _service;

    public UpdateDailySiteReportWorkerCommandHandler(IDailySiteReportWorkerService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateDailySiteReportWorkerCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
