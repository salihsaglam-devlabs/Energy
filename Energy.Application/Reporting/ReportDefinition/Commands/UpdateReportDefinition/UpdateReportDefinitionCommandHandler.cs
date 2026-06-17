using Energy.Application.Reporting.ReportDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Reporting.ReportDefinition.Commands.UpdateReportDefinition;

/// <summary>
/// <see cref="UpdateReportDefinitionCommand"/> handler'ı. <see cref="IReportDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateReportDefinitionCommandHandler
    : IRequestHandler<UpdateReportDefinitionCommand, BaseResponse<bool>>
{
    private readonly IReportDefinitionService _service;

    public UpdateReportDefinitionCommandHandler(IReportDefinitionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateReportDefinitionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
