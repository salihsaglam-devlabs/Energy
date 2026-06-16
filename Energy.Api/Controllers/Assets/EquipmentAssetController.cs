using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Assets.EquipmentAsset.Commands.CreateEquipmentAsset;
using Energy.Application.Modules.Assets.EquipmentAsset.Commands.DeleteEquipmentAsset;
using Energy.Application.Modules.Assets.EquipmentAsset.Commands.UpdateEquipmentAsset;
using Energy.Application.Modules.Assets.EquipmentAsset.Queries.GetEquipmentAssetById;
using Energy.Application.Modules.Assets.EquipmentAsset.Queries.GetEquipmentAssetList;
using Energy.Application.Modules.Assets.EquipmentAsset.Queries.GetEquipmentAssetLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Requests;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Responses;

namespace Energy.Api.Controllers.Assets;

/// <summary>
/// EquipmentAsset uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/assets/equipment-assets")]
public sealed class EquipmentAssetController : ControllerBase
{
    private readonly IMediator _mediator;

    public EquipmentAssetController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<EquipmentAssetListResponse>>>> GetList([FromQuery] GetEquipmentAssetListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEquipmentAssetListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<EquipmentAssetDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEquipmentAssetByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<EquipmentAssetLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEquipmentAssetLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateEquipmentAssetRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateEquipmentAssetCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateEquipmentAssetRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateEquipmentAssetCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteEquipmentAssetCommand(id), ct));
}
