using Energy.Shared.Models.V1.Workflow.Processes.Approval.Requests;
using Energy.Web.Clients.Workflow.Processes.Approval;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers.Workflow.Processes;

/// <summary>
/// Onay süreci (gelen kutusu) ekran denetleyicisi. Yalnızca API istemcisiyle
/// konuşur; bekleyen onayları listeler ve onayla/ret/iptal eylemlerini iletir.
/// </summary>
[Authorize]
[Route("workflow/processes/approval")]
public sealed class ApprovalProcessController : Controller
{
    private readonly IApprovalProcessApiClient _api;

    public ApprovalProcessController(IApprovalProcessApiClient api) => _api = api;

    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Workflow/Processes/Approval/Index.cshtml");

    [HttpGet("my-pending")]
    public async Task<IActionResult> MyPending(CancellationToken ct)
    {
        var envelope = await _api.GetMyPendingAsync(ct);
        return Json(new { data = envelope.Data ?? [], totalCount = envelope.Data?.Count ?? 0 });
    }

    [HttpPost("{id:guid}/approve")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Approve(Guid id, [FromBody] ApprovalActionRequest? request, CancellationToken ct)
        => Json(await _api.ApproveAsync(id, request ?? new ApprovalActionRequest(), ct));

    [HttpPost("{id:guid}/reject")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Reject(Guid id, [FromBody] ApprovalActionRequest? request, CancellationToken ct)
        => Json(await _api.RejectAsync(id, request ?? new ApprovalActionRequest(), ct));

    [HttpPost("{id:guid}/cancel")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] ApprovalActionRequest? request, CancellationToken ct)
        => Json(await _api.CancelAsync(id, request ?? new ApprovalActionRequest(), ct));
}

