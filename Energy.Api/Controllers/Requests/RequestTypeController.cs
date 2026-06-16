using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Requests.RequestType.Commands.CreateRequestType;
using Energy.Application.Modules.Requests.RequestType.Commands.DeleteRequestType;
using Energy.Application.Modules.Requests.RequestType.Commands.UpdateRequestType;
using Energy.Application.Modules.Requests.RequestType.Queries.GetRequestTypeById;
using Energy.Application.Modules.Requests.RequestType.Queries.GetRequestTypeList;
using Energy.Application.Modules.Requests.RequestType.Queries.GetRequestTypeLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestType.Requests;
using Energy.Shared.Models.V1.Requests.RequestType.Responses;

namespace Energy.Api.Controllers.Requests;

/// <summary>
/// RequestType uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/requests/request-types")]
public sealed class RequestTypeController : ControllerBase
{
    private readonly IMediator _mediator;

    public RequestTypeController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<RequestTypeListResponse>>>> GetList([FromQuery] GetRequestTypeListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRequestTypeListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<RequestTypeDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRequestTypeByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<RequestTypeLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRequestTypeLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateRequestTypeRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateRequestTypeCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateRequestTypeRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateRequestTypeCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteRequestTypeCommand(id), ct));
}
