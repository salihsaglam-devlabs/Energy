using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Operations.WorkOrder.Commands.CreateWorkOrder;
using Energy.Application.Operations.WorkOrder.Commands.DeleteWorkOrder;
using Energy.Application.Operations.WorkOrder.Commands.UpdateWorkOrder;
using Energy.Application.Operations.WorkOrder.Queries.GetWorkOrderById;
using Energy.Application.Operations.WorkOrder.Queries.GetWorkOrderList;
using Energy.Application.Operations.WorkOrder.Queries.GetWorkOrderLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrder.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrder.Responses;

namespace Energy.Api.Controllers.Operations;

/// <summary>
/// WorkOrder uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/operations/work-orders")]
public sealed class WorkOrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkOrderController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<WorkOrderListResponse>>>> GetList([FromQuery] GetWorkOrderListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<WorkOrderDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<WorkOrderLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateWorkOrderRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateWorkOrderCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateWorkOrderRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateWorkOrderCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteWorkOrderCommand(id), ct));
}
