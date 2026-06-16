using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Requests.Request.Commands.CreateRequest;
using Energy.Application.Requests.Request.Commands.DeleteRequest;
using Energy.Application.Requests.Request.Commands.UpdateRequest;
using Energy.Application.Requests.Request.Queries.GetRequestById;
using Energy.Application.Requests.Request.Queries.GetRequestList;
using Energy.Application.Requests.Request.Queries.GetRequestLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.Request.Requests;
using Energy.Shared.Models.V1.Requests.Request.Responses;

namespace Energy.Api.Controllers.Requests;

/// <summary>
/// Request uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/requests/requests")]
public sealed class RequestController : ControllerBase
{
    private readonly IMediator _mediator;

    public RequestController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<RequestListResponse>>>> GetList([FromQuery] GetRequestListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRequestListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<RequestDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRequestByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<RequestLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRequestLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateRequestRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateRequestCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateRequestRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateRequestCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteRequestCommand(id), ct));
}
