using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Operations.WorkOrderAssignment.Services;
using Energy.Application.Modules.Operations.WorkOrderAssignment.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Responses;

namespace Energy.Api.Controllers.Modules.Operations;

/// <summary>WorkOrderAssignment uç noktaları (liste, detay, lookup, create, update, delete).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/operations/work-order-assignments")]
public sealed class WorkOrderAssignmentController : ControllerBase
{
    private readonly IWorkOrderAssignmentService _service;
    private readonly IWorkOrderAssignmentLookupService _lookup;

    public WorkOrderAssignmentController(IWorkOrderAssignmentService service, IWorkOrderAssignmentLookupService lookup)
    {
        _service = service;
        _lookup = lookup;
    }

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<WorkOrderAssignmentListResponse>>>> GetList([FromQuery] GetWorkOrderAssignmentListRequest request, CancellationToken ct)
        => Ok(await _service.GetListAsync(request, ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<WorkOrderAssignmentDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<WorkOrderAssignmentLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _lookup.GetLookupAsync(search, activeOnly, ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateWorkOrderAssignmentRequest request, CancellationToken ct)
        => Ok(await _service.CreateAsync(request, ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateWorkOrderAssignmentRequest request, CancellationToken ct)
        => Ok(await _service.UpdateAsync(id, request, ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _service.DeleteAsync(id, ct));
}
