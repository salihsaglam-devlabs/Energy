using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Commands.CreateBusinessPartnerContact;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Commands.DeleteBusinessPartnerContact;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Commands.UpdateBusinessPartnerContact;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Queries.GetBusinessPartnerContactById;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Queries.GetBusinessPartnerContactList;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Queries.GetBusinessPartnerContactLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Responses;

namespace Energy.Api.Controllers.BusinessPartners;

/// <summary>
/// BusinessPartnerContact uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/business-partners/business-partner-contacts")]
public sealed class BusinessPartnerContactController : ControllerBase
{
    private readonly IMediator _mediator;

    public BusinessPartnerContactController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<BusinessPartnerContactListResponse>>>> GetList([FromQuery] GetBusinessPartnerContactListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBusinessPartnerContactListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<BusinessPartnerContactDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBusinessPartnerContactByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<BusinessPartnerContactLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBusinessPartnerContactLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateBusinessPartnerContactRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateBusinessPartnerContactCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateBusinessPartnerContactRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateBusinessPartnerContactCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteBusinessPartnerContactCommand(id), ct));
}
