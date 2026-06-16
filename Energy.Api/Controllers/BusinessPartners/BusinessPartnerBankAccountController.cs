using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Commands.CreateBusinessPartnerBankAccount;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Commands.DeleteBusinessPartnerBankAccount;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Commands.UpdateBusinessPartnerBankAccount;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Queries.GetBusinessPartnerBankAccountById;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Queries.GetBusinessPartnerBankAccountList;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Queries.GetBusinessPartnerBankAccountLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Responses;

namespace Energy.Api.Controllers.BusinessPartners;

/// <summary>
/// BusinessPartnerBankAccount uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/business-partners/business-partner-bank-accounts")]
public sealed class BusinessPartnerBankAccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public BusinessPartnerBankAccountController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<BusinessPartnerBankAccountListResponse>>>> GetList([FromQuery] GetBusinessPartnerBankAccountListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBusinessPartnerBankAccountListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<BusinessPartnerBankAccountDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBusinessPartnerBankAccountByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<BusinessPartnerBankAccountLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBusinessPartnerBankAccountLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateBusinessPartnerBankAccountRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateBusinessPartnerBankAccountCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateBusinessPartnerBankAccountRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateBusinessPartnerBankAccountCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteBusinessPartnerBankAccountCommand(id), ct));
}
