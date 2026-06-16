using Energy.Shared.Models.V1.Procurement.Processes.GoodsReceipt.Requests;
using Energy.Web.Clients.Procurement.Processes.GoodsReceipt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers.Procurement.Processes;

/// <summary>Mal kabul süreci ekran denetleyicisi (yalnızca API istemcisiyle konuşur).</summary>
[Authorize]
[Route("procurement/processes/goods-receipt")]
public sealed class GoodsReceiptProcessController : Controller
{
    private readonly IGoodsReceiptProcessApiClient _api;

    public GoodsReceiptProcessController(IGoodsReceiptProcessApiClient api) => _api = api;

    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Procurement/Processes/GoodsReceipt/Index.cshtml");

    [HttpPost("")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Execute([FromBody] GoodsReceiptProcessRequest request, CancellationToken ct)
        => Json(await _api.ExecuteAsync(request, ct));
}

