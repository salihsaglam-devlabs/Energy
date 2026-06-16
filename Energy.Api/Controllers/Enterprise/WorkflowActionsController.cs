using Asp.Versioning;
using Energy.Application.Identity.Services;
using Energy.Application.Workflow.Services;
using Energy.Domain.Workflow;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers.Enterprise;

/// <summary>
/// Onay (workflow) motoru eylemleri: süreç başlatma, onay, ret, iade, iptal ve
/// kullanıcının bekleyen onayları. Yetkilendirme uç nokta-permission eşlemesiyle
/// (Workflow.Approve / Reject / Return ...) uygulanır.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/workflow-actions")]
public sealed class WorkflowActionsController : ControllerBase
{
    private readonly IApprovalWorkflowService _workflow;
    private readonly ICurrentUser _currentUser;

    public WorkflowActionsController(IApprovalWorkflowService workflow, ICurrentUser currentUser)
    {
        _workflow = workflow;
        _currentUser = currentUser;
    }

    /// <summary>Bir kaynak nesne için onay sürecini başlatır.</summary>
    public sealed record StartBody(string RelatedModule, string RelatedEntityType, Guid RelatedEntityId, Dictionary<string, string>? Fields);

    /// <summary>Onay eylemine eşlik eden açıklama.</summary>
    public sealed record NoteBody(string? Note);

    [HttpPost("start")]
    public async Task<ActionResult<BaseResponse<ApprovalRequest?>>> Start([FromBody] StartBody body, CancellationToken ct)
    {
        if (_currentUser.UserId is not { } userId)
        {
            return Unauthorized(BaseResponse<ApprovalRequest?>.Failure("Authentication required."));
        }

        var request = await _workflow.StartAsync(
            new StartApprovalRequest(body.RelatedModule, body.RelatedEntityType, body.RelatedEntityId, userId, body.Fields), ct);
        return Ok(BaseResponse<ApprovalRequest?>.Success(request));
    }

    [HttpPost("{id:guid}/approve")]
    public Task<ActionResult<BaseResponse<ApprovalRequest>>> Approve(Guid id, [FromBody] NoteBody? body, CancellationToken ct)
        => ActAsync(id, body?.Note, (uid, note) => _workflow.ApproveAsync(id, uid, note, ct));

    [HttpPost("{id:guid}/reject")]
    public Task<ActionResult<BaseResponse<ApprovalRequest>>> Reject(Guid id, [FromBody] NoteBody? body, CancellationToken ct)
        => ActAsync(id, body?.Note, (uid, note) => _workflow.RejectAsync(id, uid, note, ct));

    [HttpPost("{id:guid}/return")]
    public Task<ActionResult<BaseResponse<ApprovalRequest>>> Return(Guid id, [FromBody] NoteBody? body, CancellationToken ct)
        => ActAsync(id, body?.Note, (uid, note) => _workflow.ReturnAsync(id, uid, note, ct));

    [HttpPost("{id:guid}/cancel")]
    public Task<ActionResult<BaseResponse<ApprovalRequest>>> Cancel(Guid id, [FromBody] NoteBody? body, CancellationToken ct)
        => ActAsync(id, body?.Note, (uid, note) => _workflow.CancelAsync(id, uid, note, ct));

    [HttpGet("my-pending")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ApprovalRequest>>>> MyPending(CancellationToken ct)
    {
        if (_currentUser.UserId is not { } userId)
        {
            return Unauthorized(BaseResponse<IReadOnlyList<ApprovalRequest>>.Failure("Authentication required."));
        }

        var pending = await _workflow.GetPendingForUserAsync(userId, ct);
        return Ok(BaseResponse<IReadOnlyList<ApprovalRequest>>.Success(pending));
    }

    private async Task<ActionResult<BaseResponse<ApprovalRequest>>> ActAsync(
        Guid id, string? note, Func<Guid, string?, Task<ApprovalRequest>> action)
    {
        if (_currentUser.UserId is not { } userId)
        {
            return Unauthorized(BaseResponse<ApprovalRequest>.Failure("Authentication required."));
        }

        try
        {
            var result = await action(userId, note);
            return Ok(BaseResponse<ApprovalRequest>.Success(result));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(BaseResponse<ApprovalRequest>.Failure(ex.Message));
        }
    }
}

