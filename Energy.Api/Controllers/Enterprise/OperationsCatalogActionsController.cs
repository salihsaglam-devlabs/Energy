using Asp.Versioning;
using Energy.Application.Catalog.Services;
using Energy.Application.Operations.Services;
using Energy.Domain.Common;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers.Enterprise;

/// <summary>Operations iş kuralı eylemleri: iş emri durum geçişi, kapatma, reopen.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/operations-actions")]
public sealed class OperationsActionsController : ControllerBase
{
    private readonly IWorkOrderService _workOrders;

    public OperationsActionsController(IWorkOrderService workOrders) => _workOrders = workOrders;

    public sealed record NoteBody(string? Note);

    [HttpPost("{id:guid}/close")]
    public async Task<ActionResult<BaseResponse<bool>>> Close(Guid id, [FromBody] NoteBody? body, CancellationToken ct)
    {
        await _workOrders.CloseAsync(id, body?.Note, ct);
        return Ok(BaseResponse<bool>.Success(true));
    }

    [HttpPost("{id:guid}/reopen")]
    public async Task<ActionResult<BaseResponse<bool>>> Reopen(Guid id, [FromBody] NoteBody? body, CancellationToken ct)
    {
        await _workOrders.ReopenAsync(id, body?.Note, ct);
        return Ok(BaseResponse<bool>.Success(true));
    }

    [HttpPost("{id:guid}/status")]
    public async Task<ActionResult<BaseResponse<bool>>> ChangeStatus(
        Guid id, [FromQuery] WorkOrderStatus status, [FromBody] NoteBody? body, CancellationToken ct)
    {
        await _workOrders.ChangeStatusAsync(id, status, body?.Note, ct);
        return Ok(BaseResponse<bool>.Success(true));
    }
}

/// <summary>Catalog iş kuralı eylemleri: öznitelik doğrulama, aktive, baz birim değişikliği.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/catalog-actions")]
public sealed class CatalogActionsController : ControllerBase
{
    private readonly IMaterialService _materials;

    public CatalogActionsController(IMaterialService materials) => _materials = materials;

    [HttpGet("{id:guid}/validate")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<string>>>> Validate(Guid id, CancellationToken ct)
        => Ok(BaseResponse<IReadOnlyList<string>>.Success(await _materials.ValidateAttributesAsync(id, ct)));

    [HttpPost("{id:guid}/activate")]
    public async Task<ActionResult<BaseResponse<bool>>> Activate(Guid id, CancellationToken ct)
    {
        await _materials.ActivateAsync(id, ct);
        return Ok(BaseResponse<bool>.Success(true));
    }

    [HttpPost("{id:guid}/base-unit")]
    public async Task<ActionResult<BaseResponse<bool>>> ChangeBaseUnit(
        Guid id, [FromQuery] Guid unitOfMeasureId, CancellationToken ct)
    {
        await _materials.ChangeBaseUnitOfMeasureAsync(id, unitOfMeasureId, ct);
        return Ok(BaseResponse<bool>.Success(true));
    }
}

