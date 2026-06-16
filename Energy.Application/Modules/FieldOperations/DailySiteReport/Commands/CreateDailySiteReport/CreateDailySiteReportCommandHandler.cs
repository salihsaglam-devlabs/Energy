using Energy.Application.Modules.FieldOperations.DailySiteReport.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReport.Commands.CreateDailySiteReport;

/// <summary>
/// <see cref="CreateDailySiteReportCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IDailySiteReportService"/>'i orkestre eder.
/// </summary>
public sealed class CreateDailySiteReportCommandHandler
    : IRequestHandler<CreateDailySiteReportCommand, BaseResponse<Guid>>
{
    private readonly IDailySiteReportService _service;

    public CreateDailySiteReportCommandHandler(IDailySiteReportService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateDailySiteReportCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
