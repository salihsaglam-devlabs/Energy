using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Operations.WorkOrderMaterialPlan.Commands.CreateWorkOrderMaterialPlan;
using Energy.Application.Operations.WorkOrderMaterialPlan.Commands.DeleteWorkOrderMaterialPlan;
using Energy.Application.Operations.WorkOrderMaterialPlan.Commands.UpdateWorkOrderMaterialPlan;
using Energy.Application.Operations.WorkOrderMaterialPlan.Queries.GetWorkOrderMaterialPlanById;
using Energy.Application.Operations.WorkOrderMaterialPlan.Queries.GetWorkOrderMaterialPlanList;
using Energy.Application.Operations.WorkOrderMaterialPlan.Queries.GetWorkOrderMaterialPlanLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Responses;

namespace Energy.Api.Controllers.Operations;

/// <summary>
/// WorkOrderMaterialPlan uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/operations/work-order-material-plans")]
public sealed class WorkOrderMaterialPlanController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkOrderMaterialPlanController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<WorkOrderMaterialPlanListResponse>>>> GetList([FromQuery] GetWorkOrderMaterialPlanListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderMaterialPlanListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<WorkOrderMaterialPlanDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderMaterialPlanByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<WorkOrderMaterialPlanLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderMaterialPlanLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateWorkOrderMaterialPlanRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateWorkOrderMaterialPlanCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateWorkOrderMaterialPlanRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateWorkOrderMaterialPlanCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteWorkOrderMaterialPlanCommand(id), ct));
}
