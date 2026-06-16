using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.FieldOperations.DailySiteReportMaterial.Commands.CreateDailySiteReportMaterial;
using Energy.Application.FieldOperations.DailySiteReportMaterial.Commands.DeleteDailySiteReportMaterial;
using Energy.Application.FieldOperations.DailySiteReportMaterial.Commands.UpdateDailySiteReportMaterial;
using Energy.Application.FieldOperations.DailySiteReportMaterial.Queries.GetDailySiteReportMaterialById;
using Energy.Application.FieldOperations.DailySiteReportMaterial.Queries.GetDailySiteReportMaterialList;
using Energy.Application.FieldOperations.DailySiteReportMaterial.Queries.GetDailySiteReportMaterialLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Responses;

namespace Energy.Api.Controllers.FieldOperations;

/// <summary>
/// DailySiteReportMaterial uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/field-operations/daily-site-report-materials")]
public sealed class DailySiteReportMaterialController : ControllerBase
{
    private readonly IMediator _mediator;

    public DailySiteReportMaterialController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<DailySiteReportMaterialListResponse>>>> GetList([FromQuery] GetDailySiteReportMaterialListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDailySiteReportMaterialListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<DailySiteReportMaterialDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDailySiteReportMaterialByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<DailySiteReportMaterialLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDailySiteReportMaterialLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateDailySiteReportMaterialRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateDailySiteReportMaterialCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateDailySiteReportMaterialRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateDailySiteReportMaterialCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteDailySiteReportMaterialCommand(id), ct));
}
