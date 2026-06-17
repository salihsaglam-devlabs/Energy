using Energy.Application.Reporting.ReportDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Reporting.ReportDefinition.Commands.DeleteReportDefinition;

/// <summary>
/// <see cref="DeleteReportDefinitionCommand"/> handler'ı. <see cref="IReportDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteReportDefinitionCommandHandler
    : IRequestHandler<DeleteReportDefinitionCommand, BaseResponse<bool>>
{
    private readonly IReportDefinitionService _service;

    public DeleteReportDefinitionCommandHandler(IReportDefinitionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteReportDefinitionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
