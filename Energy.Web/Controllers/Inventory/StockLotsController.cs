using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Inventory.StockLot.Requests;
using Energy.Web.Clients.Inventory.StockLot;

namespace Energy.Web.Controllers.Inventory;

/// <summary>StockLot ekran denetleyicisi (yalnızca API istemcisiyle konuşur).</summary>
[Authorize]
[Route("inventory/stock-lots")]
public sealed class StockLotsController : Controller
{
    private readonly IStockLotApiClient _api;

    public StockLotsController(IStockLotApiClient api) => _api = api;

    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Inventory/StockLot/Index.cshtml");

    [HttpGet("list")]
    public async Task<IActionResult> List(int skip = 0, int take = 20, string? searchValue = null, CancellationToken ct = default)
    {
        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;
        var envelope = await _api.GetListAsync(pageNumber, take <= 0 ? 20 : take, searchValue, ct);
        var page = envelope.Data;
        return Json(new { data = page?.Items ?? Array.Empty<Energy.Shared.Models.V1.Inventory.StockLot.Responses.StockLotListResponse>(), totalCount = page?.TotalCount ?? 0 });
    }

    [HttpGet("lookup")]
    public async Task<IActionResult> Lookup(string? search = null, bool activeOnly = true, CancellationToken ct = default)
        => Json((await _api.GetLookupAsync(search, activeOnly, ct)).Data ?? []);

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Detail(Guid id, CancellationToken ct)
        => Json(await _api.GetByIdAsync(id, ct));

    [HttpPost("")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Create([FromBody] CreateStockLotRequest request, CancellationToken ct)
        => Json(await _api.CreateAsync(request, ct));

    [HttpPut("{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStockLotRequest request, CancellationToken ct)
        => Json(await _api.UpdateAsync(id, request, ct));

    [HttpDelete("{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => Json(await _api.DeleteAsync(id, ct));
}
