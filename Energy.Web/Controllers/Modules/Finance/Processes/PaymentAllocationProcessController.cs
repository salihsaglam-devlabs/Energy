using Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Requests;
using Energy.Web.Clients.Modules.Finance.Processes.PaymentAllocation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers.Modules.Finance.Processes;

/// <summary>Ödeme tahsis süreci ekran denetleyicisi (master-detail; yalnızca API istemcisiyle konuşur).</summary>
[Authorize]
[Route("finance/processes/payment-allocation")]
public sealed class PaymentAllocationProcessController : Controller
{
    private readonly IPaymentAllocationProcessApiClient _api;

    public PaymentAllocationProcessController(IPaymentAllocationProcessApiClient api) => _api = api;

    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Modules/Finance/Processes/PaymentAllocation/Index.cshtml");

    [HttpPost("")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Execute([FromBody] PaymentAllocationProcessRequest request, CancellationToken ct)
        => Json(await _api.ExecuteAsync(request, ct));
}

