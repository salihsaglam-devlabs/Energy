using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Assets.EquipmentAssignment.Commands.CreateEquipmentAssignment;
using Energy.Application.Modules.Assets.EquipmentAssignment.Commands.DeleteEquipmentAssignment;
using Energy.Application.Modules.Assets.EquipmentAssignment.Commands.UpdateEquipmentAssignment;
using Energy.Application.Modules.Assets.EquipmentAssignment.Queries.GetEquipmentAssignmentById;
using Energy.Application.Modules.Assets.EquipmentAssignment.Queries.GetEquipmentAssignmentList;
using Energy.Application.Modules.Assets.EquipmentAssignment.Queries.GetEquipmentAssignmentLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Requests;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Responses;

namespace Energy.Api.Controllers.Assets;

/// <summary>
/// EquipmentAssignment uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/assets/equipment-assignments")]
public sealed class EquipmentAssignmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public EquipmentAssignmentController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<EquipmentAssignmentListResponse>>>> GetList([FromQuery] GetEquipmentAssignmentListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEquipmentAssignmentListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<EquipmentAssignmentDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEquipmentAssignmentByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<EquipmentAssignmentLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEquipmentAssignmentLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateEquipmentAssignmentRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateEquipmentAssignmentCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateEquipmentAssignmentRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateEquipmentAssignmentCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteEquipmentAssignmentCommand(id), ct));
}
