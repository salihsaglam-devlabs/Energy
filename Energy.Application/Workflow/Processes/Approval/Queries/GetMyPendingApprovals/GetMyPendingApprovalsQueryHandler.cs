using Energy.Application.Workflow.Processes.Approval;
using Energy.Application.Workflow.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.Processes.Approval.Responses;
using MediatR;

namespace Energy.Application.Workflow.Processes.Approval.Queries.GetMyPendingApprovals;

/// <summary><see cref="GetMyPendingApprovalsQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetMyPendingApprovalsQueryHandler
    : IRequestHandler<GetMyPendingApprovalsQuery, BaseResponse<IReadOnlyList<ApprovalRequestListItemResponse>>>
{
    private readonly IApprovalWorkflowService _workflow;

    public GetMyPendingApprovalsQueryHandler(IApprovalWorkflowService workflow)
        => _workflow = workflow;

    public async Task<BaseResponse<IReadOnlyList<ApprovalRequestListItemResponse>>> Handle(
        GetMyPendingApprovalsQuery request, CancellationToken ct)
    {
        var pending = await _workflow.GetPendingForUserAsync(request.UserId, ct);
        var items = pending.Select(ApprovalRequestMapper.Map).ToList();
        return BaseResponse<IReadOnlyList<ApprovalRequestListItemResponse>>.Success(items);
    }
}

