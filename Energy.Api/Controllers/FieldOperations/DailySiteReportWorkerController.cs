using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.FieldOperations.DailySiteReportWorker.Commands.CreateDailySiteReportWorker;
using Energy.Application.FieldOperations.DailySiteReportWorker.Commands.DeleteDailySiteReportWorker;
using Energy.Application.FieldOperations.DailySiteReportWorker.Commands.UpdateDailySiteReportWorker;
using Energy.Application.FieldOperations.DailySiteReportWorker.Queries.GetDailySiteReportWorkerById;
using Energy.Application.FieldOperations.DailySiteReportWorker.Queries.GetDailySiteReportWorkerList;
using Energy.Application.FieldOperations.DailySiteReportWorker.Queries.GetDailySiteReportWorkerLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Responses;

namespace Energy.Api.Controllers.FieldOperations;

/// <summary>
/// DailySiteReportWorker uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/field-operations/daily-site-report-workers")]
public sealed class DailySiteReportWorkerController : ControllerBase
{
    private readonly IMediator _mediator;

    public DailySiteReportWorkerController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<DailySiteReportWorkerListResponse>>>> GetList([FromQuery] GetDailySiteReportWorkerListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDailySiteReportWorkerListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<DailySiteReportWorkerDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDailySiteReportWorkerByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<DailySiteReportWorkerLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDailySiteReportWorkerLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateDailySiteReportWorkerRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateDailySiteReportWorkerCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateDailySiteReportWorkerRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateDailySiteReportWorkerCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteDailySiteReportWorkerCommand(id), ct));
}
