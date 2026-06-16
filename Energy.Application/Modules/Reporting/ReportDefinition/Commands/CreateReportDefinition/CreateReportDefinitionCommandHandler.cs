using Energy.Application.Modules.Reporting.ReportDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Reporting.ReportDefinition.Commands.CreateReportDefinition;

/// <summary>
/// <see cref="CreateReportDefinitionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IReportDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateReportDefinitionCommandHandler
    : IRequestHandler<CreateReportDefinitionCommand, BaseResponse<Guid>>
{
    private readonly IReportDefinitionService _service;

    public CreateReportDefinitionCommandHandler(IReportDefinitionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateReportDefinitionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
