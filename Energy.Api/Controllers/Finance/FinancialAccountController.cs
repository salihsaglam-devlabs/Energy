using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Finance.FinancialAccount.Commands.CreateFinancialAccount;
using Energy.Application.Modules.Finance.FinancialAccount.Commands.DeleteFinancialAccount;
using Energy.Application.Modules.Finance.FinancialAccount.Commands.UpdateFinancialAccount;
using Energy.Application.Modules.Finance.FinancialAccount.Queries.GetFinancialAccountById;
using Energy.Application.Modules.Finance.FinancialAccount.Queries.GetFinancialAccountList;
using Energy.Application.Modules.Finance.FinancialAccount.Queries.GetFinancialAccountLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Requests;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Responses;

namespace Energy.Api.Controllers.Finance;

/// <summary>
/// FinancialAccount uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/financial-accounts")]
public sealed class FinancialAccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public FinancialAccountController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<FinancialAccountListResponse>>>> GetList([FromQuery] GetFinancialAccountListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetFinancialAccountListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<FinancialAccountDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetFinancialAccountByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<FinancialAccountLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetFinancialAccountLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateFinancialAccountRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateFinancialAccountCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateFinancialAccountRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateFinancialAccountCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteFinancialAccountCommand(id), ct));
}
