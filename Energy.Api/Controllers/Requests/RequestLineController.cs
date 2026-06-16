using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Requests.RequestLine.Commands.CreateRequestLine;
using Energy.Application.Requests.RequestLine.Commands.DeleteRequestLine;
using Energy.Application.Requests.RequestLine.Commands.UpdateRequestLine;
using Energy.Application.Requests.RequestLine.Queries.GetRequestLineById;
using Energy.Application.Requests.RequestLine.Queries.GetRequestLineList;
using Energy.Application.Requests.RequestLine.Queries.GetRequestLineLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestLine.Requests;
using Energy.Shared.Models.V1.Requests.RequestLine.Responses;

namespace Energy.Api.Controllers.Requests;

/// <summary>
/// RequestLine uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/requests/request-lines")]
public sealed class RequestLineController : ControllerBase
{
    private readonly IMediator _mediator;

    public RequestLineController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<RequestLineListResponse>>>> GetList([FromQuery] GetRequestLineListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRequestLineListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<RequestLineDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRequestLineByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<RequestLineLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRequestLineLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateRequestLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateRequestLineCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateRequestLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateRequestLineCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteRequestLineCommand(id), ct));
}
