using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Inventory.StockBalance.Services;
using Energy.Application.Modules.Inventory.StockBalance.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockBalance.Requests;
using Energy.Shared.Models.V1.Inventory.StockBalance.Responses;

namespace Energy.Api.Controllers.Modules.Inventory;

/// <summary>StockBalance uç noktaları (liste, detay, lookup, create, update, delete).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/stock-balances")]
public sealed class StockBalanceController : ControllerBase
{
    private readonly IStockBalanceService _service;
    private readonly IStockBalanceLookupService _lookup;

    public StockBalanceController(IStockBalanceService service, IStockBalanceLookupService lookup)
    {
        _service = service;
        _lookup = lookup;
    }

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<StockBalanceListResponse>>>> GetList([FromQuery] GetStockBalanceListRequest request, CancellationToken ct)
        => Ok(await _service.GetListAsync(request, ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<StockBalanceDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<StockBalanceLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _lookup.GetLookupAsync(search, activeOnly, ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateStockBalanceRequest request, CancellationToken ct)
        => Ok(await _service.CreateAsync(request, ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateStockBalanceRequest request, CancellationToken ct)
        => Ok(await _service.UpdateAsync(id, request, ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _service.DeleteAsync(id, ct));
}
