using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.FieldOperations.DailySiteReportEquipment.Commands.CreateDailySiteReportEquipment;
using Energy.Application.Modules.FieldOperations.DailySiteReportEquipment.Commands.DeleteDailySiteReportEquipment;
using Energy.Application.Modules.FieldOperations.DailySiteReportEquipment.Commands.UpdateDailySiteReportEquipment;
using Energy.Application.Modules.FieldOperations.DailySiteReportEquipment.Queries.GetDailySiteReportEquipmentById;
using Energy.Application.Modules.FieldOperations.DailySiteReportEquipment.Queries.GetDailySiteReportEquipmentList;
using Energy.Application.Modules.FieldOperations.DailySiteReportEquipment.Queries.GetDailySiteReportEquipmentLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Responses;

namespace Energy.Api.Controllers.FieldOperations;

/// <summary>
/// DailySiteReportEquipment uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/field-operations/daily-site-report-equipments")]
public sealed class DailySiteReportEquipmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public DailySiteReportEquipmentController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<DailySiteReportEquipmentListResponse>>>> GetList([FromQuery] GetDailySiteReportEquipmentListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDailySiteReportEquipmentListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<DailySiteReportEquipmentDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDailySiteReportEquipmentByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<DailySiteReportEquipmentLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDailySiteReportEquipmentLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateDailySiteReportEquipmentRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateDailySiteReportEquipmentCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateDailySiteReportEquipmentRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateDailySiteReportEquipmentCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteDailySiteReportEquipmentCommand(id), ct));
}
