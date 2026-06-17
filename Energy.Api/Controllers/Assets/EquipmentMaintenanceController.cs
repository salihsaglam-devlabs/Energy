using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Assets.EquipmentMaintenance.Commands.CreateEquipmentMaintenance;
using Energy.Application.Assets.EquipmentMaintenance.Commands.DeleteEquipmentMaintenance;
using Energy.Application.Assets.EquipmentMaintenance.Commands.UpdateEquipmentMaintenance;
using Energy.Application.Assets.EquipmentMaintenance.Queries.GetEquipmentMaintenanceById;
using Energy.Application.Assets.EquipmentMaintenance.Queries.GetEquipmentMaintenanceList;
using Energy.Application.Assets.EquipmentMaintenance.Queries.GetEquipmentMaintenanceLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Requests;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Responses;

namespace Energy.Api.Controllers.Assets;

/// <summary>
/// EquipmentMaintenance uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/assets/equipment-maintenances")]
public sealed class EquipmentMaintenanceController : ControllerBase
{
    private readonly IMediator _mediator;

    public EquipmentMaintenanceController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<EquipmentMaintenanceListResponse>>>> GetList([FromQuery] GetEquipmentMaintenanceListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEquipmentMaintenanceListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<EquipmentMaintenanceDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEquipmentMaintenanceByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<EquipmentMaintenanceLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEquipmentMaintenanceLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateEquipmentMaintenanceRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateEquipmentMaintenanceCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateEquipmentMaintenanceRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateEquipmentMaintenanceCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteEquipmentMaintenanceCommand(id), ct));
}
