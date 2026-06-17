using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.BusinessPartners.BusinessPartnerAddress.Commands.CreateBusinessPartnerAddress;
using Energy.Application.BusinessPartners.BusinessPartnerAddress.Commands.DeleteBusinessPartnerAddress;
using Energy.Application.BusinessPartners.BusinessPartnerAddress.Commands.UpdateBusinessPartnerAddress;
using Energy.Application.BusinessPartners.BusinessPartnerAddress.Queries.GetBusinessPartnerAddressById;
using Energy.Application.BusinessPartners.BusinessPartnerAddress.Queries.GetBusinessPartnerAddressList;
using Energy.Application.BusinessPartners.BusinessPartnerAddress.Queries.GetBusinessPartnerAddressLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Responses;

namespace Energy.Api.Controllers.BusinessPartners;

/// <summary>
/// BusinessPartnerAddress uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/business-partners/business-partner-addresses")]
public sealed class BusinessPartnerAddressController : ControllerBase
{
    private readonly IMediator _mediator;

    public BusinessPartnerAddressController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<BusinessPartnerAddressListResponse>>>> GetList([FromQuery] GetBusinessPartnerAddressListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBusinessPartnerAddressListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<BusinessPartnerAddressDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBusinessPartnerAddressByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<BusinessPartnerAddressLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBusinessPartnerAddressLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateBusinessPartnerAddressRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateBusinessPartnerAddressCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateBusinessPartnerAddressRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateBusinessPartnerAddressCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteBusinessPartnerAddressCommand(id), ct));
}
