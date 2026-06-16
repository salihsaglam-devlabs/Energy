using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Finance.CostCenter.Commands.CreateCostCenter;
using Energy.Application.Finance.CostCenter.Commands.DeleteCostCenter;
using Energy.Application.Finance.CostCenter.Commands.UpdateCostCenter;
using Energy.Application.Finance.CostCenter.Queries.GetCostCenterById;
using Energy.Application.Finance.CostCenter.Queries.GetCostCenterList;
using Energy.Application.Finance.CostCenter.Queries.GetCostCenterLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CostCenter.Requests;
using Energy.Shared.Models.V1.Finance.CostCenter.Responses;

namespace Energy.Api.Controllers.Finance;

/// <summary>
/// CostCenter uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/cost-centers")]
public sealed class CostCenterController : ControllerBase
{
    private readonly IMediator _mediator;

    public CostCenterController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<CostCenterListResponse>>>> GetList([FromQuery] GetCostCenterListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCostCenterListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<CostCenterDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCostCenterByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<CostCenterLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCostCenterLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateCostCenterRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateCostCenterCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateCostCenterRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateCostCenterCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteCostCenterCommand(id), ct));
}
