using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Finance.FinancialTransaction.Commands.CreateFinancialTransaction;
using Energy.Application.Modules.Finance.FinancialTransaction.Commands.DeleteFinancialTransaction;
using Energy.Application.Modules.Finance.FinancialTransaction.Commands.UpdateFinancialTransaction;
using Energy.Application.Modules.Finance.FinancialTransaction.Queries.GetFinancialTransactionById;
using Energy.Application.Modules.Finance.FinancialTransaction.Queries.GetFinancialTransactionList;
using Energy.Application.Modules.Finance.FinancialTransaction.Queries.GetFinancialTransactionLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Requests;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Responses;

namespace Energy.Api.Controllers.Finance;

/// <summary>
/// FinancialTransaction uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/financial-transactions")]
public sealed class FinancialTransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public FinancialTransactionController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<FinancialTransactionListResponse>>>> GetList([FromQuery] GetFinancialTransactionListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetFinancialTransactionListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<FinancialTransactionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetFinancialTransactionByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<FinancialTransactionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetFinancialTransactionLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateFinancialTransactionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateFinancialTransactionCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateFinancialTransactionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateFinancialTransactionCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteFinancialTransactionCommand(id), ct));
}
