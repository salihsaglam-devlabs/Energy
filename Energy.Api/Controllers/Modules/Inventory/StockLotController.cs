using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Inventory.StockLot.Services;
using Energy.Application.Modules.Inventory.StockLot.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockLot.Requests;
using Energy.Shared.Models.V1.Inventory.StockLot.Responses;

namespace Energy.Api.Controllers.Modules.Inventory;

/// <summary>StockLot uç noktaları (liste, detay, lookup, create, update, delete).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/stock-lots")]
public sealed class StockLotController : ControllerBase
{
    private readonly IStockLotService _service;
    private readonly IStockLotLookupService _lookup;

    public StockLotController(IStockLotService service, IStockLotLookupService lookup)
    {
        _service = service;
        _lookup = lookup;
    }

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<StockLotListResponse>>>> GetList([FromQuery] GetStockLotListRequest request, CancellationToken ct)
        => Ok(await _service.GetListAsync(request, ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<StockLotDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<StockLotLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _lookup.GetLookupAsync(search, activeOnly, ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateStockLotRequest request, CancellationToken ct)
        => Ok(await _service.CreateAsync(request, ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateStockLotRequest request, CancellationToken ct)
        => Ok(await _service.UpdateAsync(id, request, ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _service.DeleteAsync(id, ct));
}
