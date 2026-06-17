using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Identity.Services;
using Energy.Application.Workflow.Processes.Approval.Commands.ApproveApproval;
using Energy.Application.Workflow.Processes.Approval.Commands.CancelApproval;
using Energy.Application.Workflow.Processes.Approval.Commands.RejectApproval;
using Energy.Application.Workflow.Processes.Approval.Queries.GetMyPendingApprovals;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.Processes.Approval.Requests;
using Energy.Shared.Models.V1.Workflow.Processes.Approval.Responses;

namespace Energy.Api.Controllers.Workflow.Processes;

/// <summary>
/// Onay süreci uç noktaları (bekleyen onaylar + onayla/ret/iptal). İş mantığı
/// transaction-güvenli handler'lar üzerinden MediatR ile yürür; controller yalnızca
/// uç nokta + oturum (auth) guard'ı yapar.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/workflow/processes/approval")]
public sealed class ApprovalProcessController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUser _currentUser;

    public ApprovalProcessController(IMediator mediator, ICurrentUser currentUser)
    {
        _mediator = mediator;
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

        return Ok(await _mediator.Send(new GetMyPendingApprovalsQuery(userId), ct));
    }

    /// <summary>Onay talebini onaylar.</summary>
    [HttpPost("{id:guid}/approve")]
    public async Task<ActionResult<BaseResponse<ApprovalRequestListItemResponse>>> Approve(Guid id, [FromBody] ApprovalActionRequest? request, CancellationToken ct)
    {
        if (_currentUser.UserId is not { } userId)
        {
            return Unauthorized(BaseResponse<ApprovalRequestListItemResponse>.Failure("Authentication required."));
        }

        return Ok(await _mediator.Send(new ApproveApprovalCommand(id, userId, request?.Note), ct));
    }

    /// <summary>Onay talebini reddeder (kaynak belge Approved olmaz).</summary>
    [HttpPost("{id:guid}/reject")]
    public async Task<ActionResult<BaseResponse<ApprovalRequestListItemResponse>>> Reject(Guid id, [FromBody] ApprovalActionRequest? request, CancellationToken ct)
    {
        if (_currentUser.UserId is not { } userId)
        {
            return Unauthorized(BaseResponse<ApprovalRequestListItemResponse>.Failure("Authentication required."));
        }

        return Ok(await _mediator.Send(new RejectApprovalCommand(id, userId, request?.Note), ct));
    }

    /// <summary>Onay talebini iptal eder.</summary>
    [HttpPost("{id:guid}/cancel")]
    public async Task<ActionResult<BaseResponse<ApprovalRequestListItemResponse>>> Cancel(Guid id, [FromBody] ApprovalActionRequest? request, CancellationToken ct)
    {
        if (_currentUser.UserId is not { } userId)
        {
            return Unauthorized(BaseResponse<ApprovalRequestListItemResponse>.Failure("Authentication required."));
        }

        return Ok(await _mediator.Send(new CancelApprovalCommand(id, userId, request?.Note), ct));
    }
}
