using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.FieldOperations.ProgressEntry.Commands.CreateProgressEntry;
using Energy.Application.FieldOperations.ProgressEntry.Commands.DeleteProgressEntry;
using Energy.Application.FieldOperations.ProgressEntry.Commands.UpdateProgressEntry;
using Energy.Application.FieldOperations.ProgressEntry.Queries.GetProgressEntryById;
using Energy.Application.FieldOperations.ProgressEntry.Queries.GetProgressEntryList;
using Energy.Application.FieldOperations.ProgressEntry.Queries.GetProgressEntryLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Requests;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Responses;

namespace Energy.Api.Controllers.FieldOperations;

/// <summary>
/// ProgressEntry uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/field-operations/progress-entries")]
public sealed class ProgressEntryController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProgressEntryController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ProgressEntryListResponse>>>> GetList([FromQuery] GetProgressEntryListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProgressEntryListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ProgressEntryDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProgressEntryByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ProgressEntryLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProgressEntryLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateProgressEntryRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateProgressEntryCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateProgressEntryRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateProgressEntryCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteProgressEntryCommand(id), ct));
}
