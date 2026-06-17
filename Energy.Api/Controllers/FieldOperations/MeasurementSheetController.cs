using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.FieldOperations.MeasurementSheet.Commands.CreateMeasurementSheet;
using Energy.Application.FieldOperations.MeasurementSheet.Commands.DeleteMeasurementSheet;
using Energy.Application.FieldOperations.MeasurementSheet.Commands.UpdateMeasurementSheet;
using Energy.Application.FieldOperations.MeasurementSheet.Queries.GetMeasurementSheetById;
using Energy.Application.FieldOperations.MeasurementSheet.Queries.GetMeasurementSheetList;
using Energy.Application.FieldOperations.MeasurementSheet.Queries.GetMeasurementSheetLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Requests;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Responses;

namespace Energy.Api.Controllers.FieldOperations;

/// <summary>
/// MeasurementSheet uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/field-operations/measurement-sheets")]
public sealed class MeasurementSheetController : ControllerBase
{
    private readonly IMediator _mediator;

    public MeasurementSheetController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<MeasurementSheetListResponse>>>> GetList([FromQuery] GetMeasurementSheetListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMeasurementSheetListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<MeasurementSheetDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMeasurementSheetByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<MeasurementSheetLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMeasurementSheetLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateMeasurementSheetRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateMeasurementSheetCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateMeasurementSheetRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateMeasurementSheetCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteMeasurementSheetCommand(id), ct));
}
