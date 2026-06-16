using Energy.Application.Modules.Workflow.Processes.Approval;
using Energy.Application.Workflow.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.Processes.Approval.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.Processes.Approval.Commands.ApproveApproval;

/// <summary><see cref="ApproveApprovalCommand"/> handler'ı (orkestrasyon, transaction-güvenli servis).</summary>
public sealed class ApproveApprovalCommandHandler
    : IRequestHandler<ApproveApprovalCommand, BaseResponse<ApprovalRequestListItemResponse>>
{
    private readonly IApprovalWorkflowService _workflow;

    public ApproveApprovalCommandHandler(IApprovalWorkflowService workflow)
        => _workflow = workflow;

    public async Task<BaseResponse<ApprovalRequestListItemResponse>> Handle(
        ApproveApprovalCommand request, CancellationToken ct)
    {
        try
        {
            var result = await _workflow.ApproveAsync(request.Id, request.ActingUserId, request.Note, ct);
            return BaseResponse<ApprovalRequestListItemResponse>.Success(ApprovalRequestMapper.Map(result));
        }
        catch (InvalidOperationException ex)
        {
            return BaseResponse<ApprovalRequestListItemResponse>.Failure(ex.Message);
        }
    }
}
