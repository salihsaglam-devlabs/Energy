using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.FieldOperations.DailySiteReport.Commands.CreateDailySiteReport;
using Energy.Application.Modules.FieldOperations.DailySiteReport.Commands.DeleteDailySiteReport;
using Energy.Application.Modules.FieldOperations.DailySiteReport.Commands.UpdateDailySiteReport;
using Energy.Application.Modules.FieldOperations.DailySiteReport.Queries.GetDailySiteReportById;
using Energy.Application.Modules.FieldOperations.DailySiteReport.Queries.GetDailySiteReportList;
using Energy.Application.Modules.FieldOperations.DailySiteReport.Queries.GetDailySiteReportLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Responses;

namespace Energy.Api.Controllers.FieldOperations;

/// <summary>
/// DailySiteReport uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/field-operations/daily-site-reports")]
public sealed class DailySiteReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public DailySiteReportController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<DailySiteReportListResponse>>>> GetList([FromQuery] GetDailySiteReportListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDailySiteReportListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<DailySiteReportDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDailySiteReportByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<DailySiteReportLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDailySiteReportLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateDailySiteReportRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateDailySiteReportCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateDailySiteReportRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateDailySiteReportCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteDailySiteReportCommand(id), ct));
}
