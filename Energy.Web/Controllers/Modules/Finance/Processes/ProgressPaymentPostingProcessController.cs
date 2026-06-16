using Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Requests;
using Energy.Web.Clients.Modules.Finance.Processes.ProgressPaymentPosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers.Modules.Finance.Processes;

/// <summary>Hakediş muhasebeleştirme süreci ekran denetleyicisi (yalnızca API istemcisiyle konuşur).</summary>
[Authorize]
[Route("finance/processes/progress-payment-posting")]
public sealed class ProgressPaymentPostingProcessController : Controller
{
    private readonly IProgressPaymentPostingProcessApiClient _api;

    public ProgressPaymentPostingProcessController(IProgressPaymentPostingProcessApiClient api) => _api = api;

    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Modules/Finance/Processes/ProgressPaymentPosting/Index.cshtml");

    [HttpPost("")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Execute([FromBody] ProgressPaymentPostingProcessRequest request, CancellationToken ct)
        => Json(await _api.ExecuteAsync(request, ct));
}

