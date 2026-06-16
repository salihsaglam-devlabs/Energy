using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Core.Currency.Commands.CreateCurrency;
using Energy.Application.Core.Currency.Commands.DeleteCurrency;
using Energy.Application.Core.Currency.Commands.UpdateCurrency;
using Energy.Application.Core.Currency.Queries.GetCurrencyById;
using Energy.Application.Core.Currency.Queries.GetCurrencyList;
using Energy.Application.Core.Currency.Queries.GetCurrencyLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Currency.Requests;
using Energy.Shared.Models.V1.Core.Currency.Responses;

namespace Energy.Api.Controllers.Core;

/// <summary>
/// Currency uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/core/currencies")]
public sealed class CurrencyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrencyController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<CurrencyListResponse>>>> GetList([FromQuery] GetCurrencyListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCurrencyListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<CurrencyDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCurrencyByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<CurrencyLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCurrencyLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateCurrencyRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateCurrencyCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateCurrencyRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateCurrencyCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteCurrencyCommand(id), ct));
}
