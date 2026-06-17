using Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Requests;
using Energy.Web.Clients.Inventory.Processes.StockIssue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers.Inventory.Processes;

/// <summary>Stok çıkış süreci ekran denetleyicisi (yalnızca API istemcisiyle konuşur).</summary>
[Authorize]
[Route("inventory/processes/stock-issue")]
public sealed class StockIssueProcessController : Controller
{
    private readonly IStockIssueProcessApiClient _api;

    public StockIssueProcessController(IStockIssueProcessApiClient api) => _api = api;

    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Inventory/Processes/StockIssue/Index.cshtml");

    [HttpPost("")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Execute([FromBody] StockIssueProcessRequest request, CancellationToken ct)
        => Json(await _api.ExecuteAsync(request, ct));
}

