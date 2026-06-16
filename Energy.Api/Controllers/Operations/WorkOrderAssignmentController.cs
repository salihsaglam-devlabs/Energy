using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Operations.WorkOrderAssignment.Commands.CreateWorkOrderAssignment;
using Energy.Application.Modules.Operations.WorkOrderAssignment.Commands.DeleteWorkOrderAssignment;
using Energy.Application.Modules.Operations.WorkOrderAssignment.Commands.UpdateWorkOrderAssignment;
using Energy.Application.Modules.Operations.WorkOrderAssignment.Queries.GetWorkOrderAssignmentById;
using Energy.Application.Modules.Operations.WorkOrderAssignment.Queries.GetWorkOrderAssignmentList;
using Energy.Application.Modules.Operations.WorkOrderAssignment.Queries.GetWorkOrderAssignmentLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Responses;

namespace Energy.Api.Controllers.Operations;

/// <summary>
/// WorkOrderAssignment uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/operations/work-order-assignments")]
public sealed class WorkOrderAssignmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkOrderAssignmentController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<WorkOrderAssignmentListResponse>>>> GetList([FromQuery] GetWorkOrderAssignmentListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderAssignmentListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<WorkOrderAssignmentDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderAssignmentByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<WorkOrderAssignmentLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderAssignmentLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateWorkOrderAssignmentRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateWorkOrderAssignmentCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateWorkOrderAssignmentRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateWorkOrderAssignmentCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteWorkOrderAssignmentCommand(id), ct));
}
