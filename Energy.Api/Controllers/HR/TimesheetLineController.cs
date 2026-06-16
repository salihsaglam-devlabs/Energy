using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.HR.TimesheetLine.Commands.CreateTimesheetLine;
using Energy.Application.HR.TimesheetLine.Commands.DeleteTimesheetLine;
using Energy.Application.HR.TimesheetLine.Commands.UpdateTimesheetLine;
using Energy.Application.HR.TimesheetLine.Queries.GetTimesheetLineById;
using Energy.Application.HR.TimesheetLine.Queries.GetTimesheetLineList;
using Energy.Application.HR.TimesheetLine.Queries.GetTimesheetLineLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.TimesheetLine.Requests;
using Energy.Shared.Models.V1.HR.TimesheetLine.Responses;

namespace Energy.Api.Controllers.HR;

/// <summary>
/// TimesheetLine uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/h-r/timesheet-lines")]
public sealed class TimesheetLineController : ControllerBase
{
    private readonly IMediator _mediator;

    public TimesheetLineController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<TimesheetLineListResponse>>>> GetList([FromQuery] GetTimesheetLineListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetTimesheetLineListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<TimesheetLineDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetTimesheetLineByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<TimesheetLineLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetTimesheetLineLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateTimesheetLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateTimesheetLineCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateTimesheetLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateTimesheetLineCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteTimesheetLineCommand(id), ct));
}
