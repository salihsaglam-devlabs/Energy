using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Operations.WorkOrderMaterialUsage.Commands.CreateWorkOrderMaterialUsage;
using Energy.Application.Modules.Operations.WorkOrderMaterialUsage.Commands.DeleteWorkOrderMaterialUsage;
using Energy.Application.Modules.Operations.WorkOrderMaterialUsage.Commands.UpdateWorkOrderMaterialUsage;
using Energy.Application.Modules.Operations.WorkOrderMaterialUsage.Queries.GetWorkOrderMaterialUsageById;
using Energy.Application.Modules.Operations.WorkOrderMaterialUsage.Queries.GetWorkOrderMaterialUsageList;
using Energy.Application.Modules.Operations.WorkOrderMaterialUsage.Queries.GetWorkOrderMaterialUsageLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Responses;

namespace Energy.Api.Controllers.Operations;

/// <summary>
/// WorkOrderMaterialUsage uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/operations/work-order-material-usages")]
public sealed class WorkOrderMaterialUsageController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkOrderMaterialUsageController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<WorkOrderMaterialUsageListResponse>>>> GetList([FromQuery] GetWorkOrderMaterialUsageListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderMaterialUsageListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<WorkOrderMaterialUsageDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderMaterialUsageByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<WorkOrderMaterialUsageLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkOrderMaterialUsageLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateWorkOrderMaterialUsageRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateWorkOrderMaterialUsageCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateWorkOrderMaterialUsageRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateWorkOrderMaterialUsageCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteWorkOrderMaterialUsageCommand(id), ct));
}
