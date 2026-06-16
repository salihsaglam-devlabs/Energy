using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Operations.WorkOrderChecklist.Commands.CreateWorkOrderChecklist;
using Energy.Application.Modules.Operations.WorkOrderChecklist.Commands.DeleteWorkOrderChecklist;
using Energy.Application.Modules.Operations.WorkOrderChecklist.Commands.UpdateWorkOrderChecklist;
using Energy.Application.Modules.Operations.WorkOrderChecklist.Queries.GetWorkOrderChecklistById;
using Energy.Application.Modules.Operations.WorkOrderChecklist.Queries.GetWorkOrderChecklistList;
using Energy.Application.Modules.Operations.WorkOrderChecklist.Queries.GetWorkOrderChecklistLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Responses;

namespace Energy.Api.Controllers.Operations;

/// <summary>
/// WorkOrderChecklist uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/operations/work-order-checklists")]
public sealed class WorkOrderChecklistController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkOrderChecklistController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<WorkOrderChecklistListResponse>>>> GetList([FromQuery] GetWorkOrderChecklistListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderChecklistListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<WorkOrderChecklistDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderChecklistByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<WorkOrderChecklistLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderChecklistLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateWorkOrderChecklistRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateWorkOrderChecklistCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateWorkOrderChecklistRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateWorkOrderChecklistCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteWorkOrderChecklistCommand(id), ct));
}
