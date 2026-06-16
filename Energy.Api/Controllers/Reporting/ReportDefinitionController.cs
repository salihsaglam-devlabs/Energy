using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Reporting.ReportDefinition.Commands.CreateReportDefinition;
using Energy.Application.Modules.Reporting.ReportDefinition.Commands.DeleteReportDefinition;
using Energy.Application.Modules.Reporting.ReportDefinition.Commands.UpdateReportDefinition;
using Energy.Application.Modules.Reporting.ReportDefinition.Queries.GetReportDefinitionById;
using Energy.Application.Modules.Reporting.ReportDefinition.Queries.GetReportDefinitionList;
using Energy.Application.Modules.Reporting.ReportDefinition.Queries.GetReportDefinitionLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Requests;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Responses;

namespace Energy.Api.Controllers.Reporting;

/// <summary>
/// ReportDefinition uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/reporting/report-definitions")]
public sealed class ReportDefinitionController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportDefinitionController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ReportDefinitionListResponse>>>> GetList([FromQuery] GetReportDefinitionListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetReportDefinitionListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ReportDefinitionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetReportDefinitionByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ReportDefinitionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetReportDefinitionLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateReportDefinitionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateReportDefinitionCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateReportDefinitionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateReportDefinitionCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteReportDefinitionCommand(id), ct));
}
