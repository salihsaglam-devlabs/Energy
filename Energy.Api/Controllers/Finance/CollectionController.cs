using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Finance.Collection.Commands.CreateCollection;
using Energy.Application.Finance.Collection.Commands.DeleteCollection;
using Energy.Application.Finance.Collection.Commands.UpdateCollection;
using Energy.Application.Finance.Collection.Queries.GetCollectionById;
using Energy.Application.Finance.Collection.Queries.GetCollectionList;
using Energy.Application.Finance.Collection.Queries.GetCollectionLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Collection.Requests;
using Energy.Shared.Models.V1.Finance.Collection.Responses;

namespace Energy.Api.Controllers.Finance;

/// <summary>
/// Collection uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/collections")]
public sealed class CollectionController : ControllerBase
{
    private readonly IMediator _mediator;

    public CollectionController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<CollectionListResponse>>>> GetList([FromQuery] GetCollectionListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCollectionListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<CollectionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCollectionByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<CollectionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCollectionLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateCollectionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateCollectionCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateCollectionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateCollectionCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteCollectionCommand(id), ct));
}
