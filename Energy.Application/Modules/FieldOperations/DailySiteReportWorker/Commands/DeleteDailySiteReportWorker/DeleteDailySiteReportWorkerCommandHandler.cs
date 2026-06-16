using Energy.Application.Modules.FieldOperations.DailySiteReportWorker.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportWorker.Commands.DeleteDailySiteReportWorker;

/// <summary>
/// <see cref="DeleteDailySiteReportWorkerCommand"/> handler'ı. <see cref="IDailySiteReportWorkerService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteDailySiteReportWorkerCommandHandler
    : IRequestHandler<DeleteDailySiteReportWorkerCommand, BaseResponse<bool>>
{
    private readonly IDailySiteReportWorkerService _service;

    public DeleteDailySiteReportWorkerCommandHandler(IDailySiteReportWorkerService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteDailySiteReportWorkerCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
