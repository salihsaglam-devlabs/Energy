using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Operations.WorkOrderChecklistItem.Commands.CreateWorkOrderChecklistItem;
using Energy.Application.Modules.Operations.WorkOrderChecklistItem.Commands.DeleteWorkOrderChecklistItem;
using Energy.Application.Modules.Operations.WorkOrderChecklistItem.Commands.UpdateWorkOrderChecklistItem;
using Energy.Application.Modules.Operations.WorkOrderChecklistItem.Queries.GetWorkOrderChecklistItemById;
using Energy.Application.Modules.Operations.WorkOrderChecklistItem.Queries.GetWorkOrderChecklistItemList;
using Energy.Application.Modules.Operations.WorkOrderChecklistItem.Queries.GetWorkOrderChecklistItemLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Responses;

namespace Energy.Api.Controllers.Operations;

/// <summary>
/// WorkOrderChecklistItem uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/operations/work-order-checklist-items")]
public sealed class WorkOrderChecklistItemController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkOrderChecklistItemController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<WorkOrderChecklistItemListResponse>>>> GetList([FromQuery] GetWorkOrderChecklistItemListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderChecklistItemListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<WorkOrderChecklistItemDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderChecklistItemByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<WorkOrderChecklistItemLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderChecklistItemLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateWorkOrderChecklistItemRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateWorkOrderChecklistItemCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateWorkOrderChecklistItemRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateWorkOrderChecklistItemCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteWorkOrderChecklistItemCommand(id), ct));
}
