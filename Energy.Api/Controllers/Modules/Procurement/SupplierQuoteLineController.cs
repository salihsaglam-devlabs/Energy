using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Procurement.SupplierQuoteLine.Services;
using Energy.Application.Modules.Procurement.SupplierQuoteLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Responses;

namespace Energy.Api.Controllers.Modules.Procurement;

/// <summary>SupplierQuoteLine uç noktaları (liste, detay, lookup, create, update, delete).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/procurement/supplier-quote-lines")]
public sealed class SupplierQuoteLineController : ControllerBase
{
    private readonly ISupplierQuoteLineService _service;
    private readonly ISupplierQuoteLineLookupService _lookup;

    public SupplierQuoteLineController(ISupplierQuoteLineService service, ISupplierQuoteLineLookupService lookup)
    {
        _service = service;
        _lookup = lookup;
    }

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<SupplierQuoteLineListResponse>>>> GetList([FromQuery] GetSupplierQuoteLineListRequest request, CancellationToken ct)
        => Ok(await _service.GetListAsync(request, ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<SupplierQuoteLineDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<SupplierQuoteLineLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _lookup.GetLookupAsync(search, activeOnly, ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateSupplierQuoteLineRequest request, CancellationToken ct)
        => Ok(await _service.CreateAsync(request, ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateSupplierQuoteLineRequest request, CancellationToken ct)
        => Ok(await _service.UpdateAsync(id, request, ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _service.DeleteAsync(id, ct));
}
