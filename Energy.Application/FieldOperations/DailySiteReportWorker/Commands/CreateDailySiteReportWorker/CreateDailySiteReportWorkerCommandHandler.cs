using Energy.Application.FieldOperations.DailySiteReportWorker.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportWorker.Commands.CreateDailySiteReportWorker;

/// <summary>
/// <see cref="CreateDailySiteReportWorkerCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IDailySiteReportWorkerService"/>'i orkestre eder.
/// </summary>
public sealed class CreateDailySiteReportWorkerCommandHandler
    : IRequestHandler<CreateDailySiteReportWorkerCommand, BaseResponse<Guid>>
{
    private readonly IDailySiteReportWorkerService _service;

    public CreateDailySiteReportWorkerCommandHandler(IDailySiteReportWorkerService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateDailySiteReportWorkerCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
