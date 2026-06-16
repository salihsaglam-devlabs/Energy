using Asp.Versioning;
using Energy.Application.Identity.Services;
using Energy.Application.Workflow.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.Processes.Approval.Requests;
using Energy.Shared.Models.V1.Workflow.Processes.Approval.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers.Modules.Workflow.Processes;

/// <summary>
/// Onay süreci uç noktaları (standart süreç rotası). Bekleyen onaylar + onayla/
/// ret/iptal eylemleri. İş mantığı transaction-güvenli <see cref="IApprovalWorkflowService"/>
/// üzerinden yürür; bu controller yalnızca uç nokta + Shared DTO eşlemesi yapar.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/workflow/processes/approval")]
public sealed class ApprovalProcessController : ControllerBase
{
    private readonly IApprovalWorkflowService _workflow;
    private readonly ICurrentUser _currentUser;

    public ApprovalProcessController(IApprovalWorkflowService workflow, ICurrentUser currentUser)
    {
        _workflow = workflow;
        _currentUser = currentUser;
    }

    /// <summary>Oturum sahibinin işlem yapması beklenen bekleyen onaylar.</summary>
    [HttpGet("my-pending")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ApprovalRequestListItemResponse>>>> MyPending(CancellationToken ct)
    {
        if (_currentUser.UserId is not { } userId)
        {
            return Unauthorized(BaseResponse<IReadOnlyList<ApprovalRequestListItemResponse>>.Failure("Authentication required."));
        }

        var pending = await _workflow.GetPendingForUserAsync(userId, ct);
        var items = pending.Select(Map).ToList();
        return Ok(BaseResponse<IReadOnlyList<ApprovalRequestListItemResponse>>.Success(items));
    }

    /// <summary>Onay talebini onaylar (sıradaki adıma ilerletir / tamamlar).</summary>
    [HttpPost("{id:guid}/approve")]
    public Task<ActionResult<BaseResponse<ApprovalRequestListItemResponse>>> Approve(Guid id, [FromBody] ApprovalActionRequest? request, CancellationToken ct)
        => ActAsync(id, (uid, note) => _workflow.ApproveAsync(id, uid, note, ct), request?.Note);

    /// <summary>Onay talebini reddeder (kaynak belge Approved olmaz).</summary>
    [HttpPost("{id:guid}/reject")]
    public Task<ActionResult<BaseResponse<ApprovalRequestListItemResponse>>> Reject(Guid id, [FromBody] ApprovalActionRequest? request, CancellationToken ct)
        => ActAsync(id, (uid, note) => _workflow.RejectAsync(id, uid, note, ct), request?.Note);

    /// <summary>Onay talebini iptal eder.</summary>
    [HttpPost("{id:guid}/cancel")]
    public Task<ActionResult<BaseResponse<ApprovalRequestListItemResponse>>> Cancel(Guid id, [FromBody] ApprovalActionRequest? request, CancellationToken ct)
        => ActAsync(id, (uid, note) => _workflow.CancelAsync(id, uid, note, ct), request?.Note);

    private async Task<ActionResult<BaseResponse<ApprovalRequestListItemResponse>>> ActAsync(
        Guid id, Func<Guid, string?, Task<Energy.Domain.Modules.Workflow.ApprovalRequest>> action, string? note)
    {
        if (_currentUser.UserId is not { } userId)
        {
            return Unauthorized(BaseResponse<ApprovalRequestListItemResponse>.Failure("Authentication required."));
        }

        try
        {
            var result = await action(userId, note);
            return Ok(BaseResponse<ApprovalRequestListItemResponse>.Success(Map(result)));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(BaseResponse<ApprovalRequestListItemResponse>.Failure(ex.Message));
        }
    }

    private static ApprovalRequestListItemResponse Map(Energy.Domain.Modules.Workflow.ApprovalRequest r) => new()
    {
        Id = r.Id,
        RelatedModule = r.RelatedModule,
        RelatedEntityType = r.RelatedEntityType,
        RelatedEntityId = r.RelatedEntityId,
        Status = r.Status.ToString(),
        CurrentStepNo = r.CurrentStepNo,
        CreatedAt = r.CreatedAt,
    };
}

