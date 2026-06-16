using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Services;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Responses;

namespace Energy.Api.Controllers.Modules.BusinessPartners;

/// <summary>BusinessPartnerContact uç noktaları (liste, detay, lookup, create, update, delete).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/business-partners/business-partner-contacts")]
public sealed class BusinessPartnerContactController : ControllerBase
{
    private readonly IBusinessPartnerContactService _service;
    private readonly IBusinessPartnerContactLookupService _lookup;

    public BusinessPartnerContactController(IBusinessPartnerContactService service, IBusinessPartnerContactLookupService lookup)
    {
        _service = service;
        _lookup = lookup;
    }

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<BusinessPartnerContactListResponse>>>> GetList([FromQuery] GetBusinessPartnerContactListRequest request, CancellationToken ct)
        => Ok(await _service.GetListAsync(request, ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<BusinessPartnerContactDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<BusinessPartnerContactLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _lookup.GetLookupAsync(search, activeOnly, ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateBusinessPartnerContactRequest request, CancellationToken ct)
        => Ok(await _service.CreateAsync(request, ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateBusinessPartnerContactRequest request, CancellationToken ct)
        => Ok(await _service.UpdateAsync(id, request, ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _service.DeleteAsync(id, ct));
}
