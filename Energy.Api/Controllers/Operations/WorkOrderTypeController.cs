using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Operations.WorkOrderType.Commands.CreateWorkOrderType;
using Energy.Application.Modules.Operations.WorkOrderType.Commands.DeleteWorkOrderType;
using Energy.Application.Modules.Operations.WorkOrderType.Commands.UpdateWorkOrderType;
using Energy.Application.Modules.Operations.WorkOrderType.Queries.GetWorkOrderTypeById;
using Energy.Application.Modules.Operations.WorkOrderType.Queries.GetWorkOrderTypeList;
using Energy.Application.Modules.Operations.WorkOrderType.Queries.GetWorkOrderTypeLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderType.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderType.Responses;

namespace Energy.Api.Controllers.Operations;

/// <summary>
/// WorkOrderType uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/operations/work-order-types")]
public sealed class WorkOrderTypeController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkOrderTypeController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<WorkOrderTypeListResponse>>>> GetList([FromQuery] GetWorkOrderTypeListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderTypeListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<WorkOrderTypeDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderTypeByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<WorkOrderTypeLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderTypeLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateWorkOrderTypeRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateWorkOrderTypeCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateWorkOrderTypeRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateWorkOrderTypeCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteWorkOrderTypeCommand(id), ct));
}
