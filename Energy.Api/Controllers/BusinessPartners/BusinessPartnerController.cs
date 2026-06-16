using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.BusinessPartners.BusinessPartner.Commands.CreateBusinessPartner;
using Energy.Application.BusinessPartners.BusinessPartner.Commands.DeleteBusinessPartner;
using Energy.Application.BusinessPartners.BusinessPartner.Commands.UpdateBusinessPartner;
using Energy.Application.BusinessPartners.BusinessPartner.Queries.GetBusinessPartnerById;
using Energy.Application.BusinessPartners.BusinessPartner.Queries.GetBusinessPartnerList;
using Energy.Application.BusinessPartners.BusinessPartner.Queries.GetBusinessPartnerLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Responses;

namespace Energy.Api.Controllers.BusinessPartners;

/// <summary>
/// BusinessPartner uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/business-partners/business-partners")]
public sealed class BusinessPartnerController : ControllerBase
{
    private readonly IMediator _mediator;

    public BusinessPartnerController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<BusinessPartnerListResponse>>>> GetList([FromQuery] GetBusinessPartnerListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBusinessPartnerListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<BusinessPartnerDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBusinessPartnerByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<BusinessPartnerLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBusinessPartnerLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateBusinessPartnerRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateBusinessPartnerCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateBusinessPartnerRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateBusinessPartnerCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteBusinessPartnerCommand(id), ct));
}
