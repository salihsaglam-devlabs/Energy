using Energy.Application.FieldOperations.DailySiteReport.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReport.Commands.UpdateDailySiteReport;

/// <summary>
/// <see cref="UpdateDailySiteReportCommand"/> handler'ı. <see cref="IDailySiteReportService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateDailySiteReportCommandHandler
    : IRequestHandler<UpdateDailySiteReportCommand, BaseResponse<bool>>
{
    private readonly IDailySiteReportService _service;

    public UpdateDailySiteReportCommandHandler(IDailySiteReportService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateDailySiteReportCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
