using Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Requests;
using Energy.Web.Clients.Inventory.Processes.StockTransfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers.Inventory.Processes;

/// <summary>Stok transfer süreci ekran denetleyicisi (yalnızca API istemcisiyle konuşur).</summary>
[Authorize]
[Route("inventory/processes/stock-transfer")]
public sealed class StockTransferProcessController : Controller
{
    private readonly IStockTransferProcessApiClient _api;

    public StockTransferProcessController(IStockTransferProcessApiClient api) => _api = api;

    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Inventory/Processes/StockTransfer/Index.cshtml");

    [HttpPost("")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Execute([FromBody] StockTransferProcessRequest request, CancellationToken ct)
        => Json(await _api.ExecuteAsync(request, ct));
}

