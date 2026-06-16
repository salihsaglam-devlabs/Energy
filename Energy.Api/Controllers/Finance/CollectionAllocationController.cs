using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Finance.CollectionAllocation.Commands.CreateCollectionAllocation;
using Energy.Application.Modules.Finance.CollectionAllocation.Commands.DeleteCollectionAllocation;
using Energy.Application.Modules.Finance.CollectionAllocation.Commands.UpdateCollectionAllocation;
using Energy.Application.Modules.Finance.CollectionAllocation.Queries.GetCollectionAllocationById;
using Energy.Application.Modules.Finance.CollectionAllocation.Queries.GetCollectionAllocationList;
using Energy.Application.Modules.Finance.CollectionAllocation.Queries.GetCollectionAllocationLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Requests;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Responses;

namespace Energy.Api.Controllers.Finance;

/// <summary>
/// CollectionAllocation uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/collection-allocations")]
public sealed class CollectionAllocationController : ControllerBase
{
    private readonly IMediator _mediator;

    public CollectionAllocationController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<CollectionAllocationListResponse>>>> GetList([FromQuery] GetCollectionAllocationListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCollectionAllocationListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<CollectionAllocationDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCollectionAllocationByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<CollectionAllocationLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCollectionAllocationLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateCollectionAllocationRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateCollectionAllocationCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateCollectionAllocationRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateCollectionAllocationCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteCollectionAllocationCommand(id), ct));
}
