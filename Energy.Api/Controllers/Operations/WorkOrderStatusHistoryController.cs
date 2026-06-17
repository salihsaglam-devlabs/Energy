using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Operations.WorkOrderStatusHistory.Commands.CreateWorkOrderStatusHistory;
using Energy.Application.Operations.WorkOrderStatusHistory.Commands.DeleteWorkOrderStatusHistory;
using Energy.Application.Operations.WorkOrderStatusHistory.Commands.UpdateWorkOrderStatusHistory;
using Energy.Application.Operations.WorkOrderStatusHistory.Queries.GetWorkOrderStatusHistoryById;
using Energy.Application.Operations.WorkOrderStatusHistory.Queries.GetWorkOrderStatusHistoryList;
using Energy.Application.Operations.WorkOrderStatusHistory.Queries.GetWorkOrderStatusHistoryLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Responses;

namespace Energy.Api.Controllers.Operations;

/// <summary>
/// WorkOrderStatusHistory uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/operations/work-order-status-histories")]
public sealed class WorkOrderStatusHistoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkOrderStatusHistoryController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<WorkOrderStatusHistoryListResponse>>>> GetList([FromQuery] GetWorkOrderStatusHistoryListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderStatusHistoryListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<WorkOrderStatusHistoryDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderStatusHistoryByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<WorkOrderStatusHistoryLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderStatusHistoryLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateWorkOrderStatusHistoryRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateWorkOrderStatusHistoryCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateWorkOrderStatusHistoryRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateWorkOrderStatusHistoryCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteWorkOrderStatusHistoryCommand(id), ct));
}
