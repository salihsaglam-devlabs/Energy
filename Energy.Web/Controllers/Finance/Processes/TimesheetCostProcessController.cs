using Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Requests;
using Energy.Web.Clients.Finance.Processes.TimesheetCost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers.Finance.Processes;

/// <summary>Puantaj maliyet süreci ekran denetleyicisi (yalnızca API istemcisiyle konuşur).</summary>
[Authorize]
[Route("finance/processes/timesheet-cost")]
public sealed class TimesheetCostProcessController : Controller
{
    private readonly ITimesheetCostProcessApiClient _api;

    public TimesheetCostProcessController(ITimesheetCostProcessApiClient api) => _api = api;

    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Finance/Processes/TimesheetCost/Index.cshtml");

    [HttpPost("")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Execute([FromBody] TimesheetCostProcessRequest request, CancellationToken ct)
        => Json(await _api.ExecuteAsync(request, ct));
}

