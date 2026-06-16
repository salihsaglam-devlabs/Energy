using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Core.ExchangeRate.Commands.CreateExchangeRate;
using Energy.Application.Core.ExchangeRate.Commands.DeleteExchangeRate;
using Energy.Application.Core.ExchangeRate.Commands.UpdateExchangeRate;
using Energy.Application.Core.ExchangeRate.Queries.GetExchangeRateById;
using Energy.Application.Core.ExchangeRate.Queries.GetExchangeRateList;
using Energy.Application.Core.ExchangeRate.Queries.GetExchangeRateLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.ExchangeRate.Requests;
using Energy.Shared.Models.V1.Core.ExchangeRate.Responses;

namespace Energy.Api.Controllers.Core;

/// <summary>
/// ExchangeRate uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/core/exchange-rates")]
public sealed class ExchangeRateController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExchangeRateController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ExchangeRateListResponse>>>> GetList([FromQuery] GetExchangeRateListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetExchangeRateListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ExchangeRateDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetExchangeRateByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ExchangeRateLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetExchangeRateLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateExchangeRateRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateExchangeRateCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateExchangeRateRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateExchangeRateCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteExchangeRateCommand(id), ct));
}
