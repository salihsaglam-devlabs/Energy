using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Inventory.StockBalance.Requests;
using Energy.Web.Clients.Inventory.StockBalance;

namespace Energy.Web.Controllers.Inventory;

/// <summary>StockBalance ekran denetleyicisi (yalnızca API istemcisiyle konuşur).</summary>
[Authorize]
[Route("inventory/stock-balances")]
public sealed class StockBalancesController : Controller
{
    private readonly IStockBalanceApiClient _api;

    public StockBalancesController(IStockBalanceApiClient api) => _api = api;

    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Inventory/StockBalance/Index.cshtml");

    [HttpGet("list")]
    public async Task<IActionResult> List(int skip = 0, int take = 20, string? searchValue = null, CancellationToken ct = default)
    {
        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;
        var envelope = await _api.GetListAsync(pageNumber, take <= 0 ? 20 : take, searchValue, ct);
        var page = envelope.Data;
        return Json(new { data = page?.Items ?? Array.Empty<Energy.Shared.Models.V1.Inventory.StockBalance.Responses.StockBalanceListResponse>(), totalCount = page?.TotalCount ?? 0 });
    }

    [HttpGet("lookup")]
    public async Task<IActionResult> Lookup(string? search = null, bool activeOnly = true, CancellationToken ct = default)
        => Json((await _api.GetLookupAsync(search, activeOnly, ct)).Data ?? []);

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Detail(Guid id, CancellationToken ct)
        => Json(await _api.GetByIdAsync(id, ct));

    [HttpPost("")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Create([FromBody] CreateStockBalanceRequest request, CancellationToken ct)
        => Json(await _api.CreateAsync(request, ct));

    [HttpPut("{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStockBalanceRequest request, CancellationToken ct)
        => Json(await _api.UpdateAsync(id, request, ct));

    [HttpDelete("{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => Json(await _api.DeleteAsync(id, ct));
}
