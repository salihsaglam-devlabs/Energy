using Energy.Application.Reporting.DashboardWidget.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Reporting.DashboardWidget.Commands.CreateDashboardWidget;

/// <summary>
/// <see cref="CreateDashboardWidgetCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IDashboardWidgetService"/>'i orkestre eder.
/// </summary>
public sealed class CreateDashboardWidgetCommandHandler
    : IRequestHandler<CreateDashboardWidgetCommand, BaseResponse<Guid>>
{
    private readonly IDashboardWidgetService _service;

    public CreateDashboardWidgetCommandHandler(IDashboardWidgetService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateDashboardWidgetCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
