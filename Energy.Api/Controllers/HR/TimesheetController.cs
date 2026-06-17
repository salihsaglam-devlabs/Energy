using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.HR.Timesheet.Commands.CreateTimesheet;
using Energy.Application.HR.Timesheet.Commands.DeleteTimesheet;
using Energy.Application.HR.Timesheet.Commands.UpdateTimesheet;
using Energy.Application.HR.Timesheet.Queries.GetTimesheetById;
using Energy.Application.HR.Timesheet.Queries.GetTimesheetList;
using Energy.Application.HR.Timesheet.Queries.GetTimesheetLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Timesheet.Requests;
using Energy.Shared.Models.V1.HR.Timesheet.Responses;

namespace Energy.Api.Controllers.HR;

/// <summary>
/// Timesheet uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/h-r/timesheets")]
public sealed class TimesheetController : ControllerBase
{
    private readonly IMediator _mediator;

    public TimesheetController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<TimesheetListResponse>>>> GetList([FromQuery] GetTimesheetListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetTimesheetListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<TimesheetDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetTimesheetByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<TimesheetLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetTimesheetLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateTimesheetRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateTimesheetCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateTimesheetRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateTimesheetCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteTimesheetCommand(id), ct));
}
