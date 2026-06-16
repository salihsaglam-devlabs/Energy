using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Reporting.DashboardWidget.Commands.CreateDashboardWidget;
using Energy.Application.Modules.Reporting.DashboardWidget.Commands.DeleteDashboardWidget;
using Energy.Application.Modules.Reporting.DashboardWidget.Commands.UpdateDashboardWidget;
using Energy.Application.Modules.Reporting.DashboardWidget.Queries.GetDashboardWidgetById;
using Energy.Application.Modules.Reporting.DashboardWidget.Queries.GetDashboardWidgetList;
using Energy.Application.Modules.Reporting.DashboardWidget.Queries.GetDashboardWidgetLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Requests;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Responses;

namespace Energy.Api.Controllers.Reporting;

/// <summary>
/// DashboardWidget uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/reporting/dashboard-widgets")]
public sealed class DashboardWidgetController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardWidgetController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<DashboardWidgetListResponse>>>> GetList([FromQuery] GetDashboardWidgetListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDashboardWidgetListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<DashboardWidgetDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDashboardWidgetByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<DashboardWidgetLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDashboardWidgetLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateDashboardWidgetRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateDashboardWidgetCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateDashboardWidgetRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateDashboardWidgetCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteDashboardWidgetCommand(id), ct));
}
