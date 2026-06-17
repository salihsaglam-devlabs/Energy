using Energy.Application.FieldOperations.DailySiteReport.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReport.Commands.DeleteDailySiteReport;

/// <summary>
/// <see cref="DeleteDailySiteReportCommand"/> handler'ı. <see cref="IDailySiteReportService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteDailySiteReportCommandHandler
    : IRequestHandler<DeleteDailySiteReportCommand, BaseResponse<bool>>
{
    private readonly IDailySiteReportService _service;

    public DeleteDailySiteReportCommandHandler(IDailySiteReportService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteDailySiteReportCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
