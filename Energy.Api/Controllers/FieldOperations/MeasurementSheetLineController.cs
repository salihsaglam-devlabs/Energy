using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.FieldOperations.MeasurementSheetLine.Commands.CreateMeasurementSheetLine;
using Energy.Application.FieldOperations.MeasurementSheetLine.Commands.DeleteMeasurementSheetLine;
using Energy.Application.FieldOperations.MeasurementSheetLine.Commands.UpdateMeasurementSheetLine;
using Energy.Application.FieldOperations.MeasurementSheetLine.Queries.GetMeasurementSheetLineById;
using Energy.Application.FieldOperations.MeasurementSheetLine.Queries.GetMeasurementSheetLineList;
using Energy.Application.FieldOperations.MeasurementSheetLine.Queries.GetMeasurementSheetLineLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Requests;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Responses;

namespace Energy.Api.Controllers.FieldOperations;

/// <summary>
/// MeasurementSheetLine uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/field-operations/measurement-sheet-lines")]
public sealed class MeasurementSheetLineController : ControllerBase
{
    private readonly IMediator _mediator;

    public MeasurementSheetLineController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<MeasurementSheetLineListResponse>>>> GetList([FromQuery] GetMeasurementSheetLineListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMeasurementSheetLineListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<MeasurementSheetLineDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMeasurementSheetLineByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<MeasurementSheetLineLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMeasurementSheetLineLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateMeasurementSheetLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateMeasurementSheetLineCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateMeasurementSheetLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateMeasurementSheetLineCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteMeasurementSheetLineCommand(id), ct));
}
