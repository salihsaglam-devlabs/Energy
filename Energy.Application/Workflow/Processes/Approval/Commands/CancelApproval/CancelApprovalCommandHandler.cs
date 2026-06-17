using Energy.Application.Workflow.Processes.Approval;
using Energy.Application.Workflow.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.Processes.Approval.Responses;
using MediatR;

namespace Energy.Application.Workflow.Processes.Approval.Commands.CancelApproval;

/// <summary><see cref="CancelApprovalCommand"/> handler'ı (orkestrasyon, transaction-güvenli servis).</summary>
public sealed class CancelApprovalCommandHandler
    : IRequestHandler<CancelApprovalCommand, BaseResponse<ApprovalRequestListItemResponse>>
{
    private readonly IApprovalWorkflowService _workflow;

    public CancelApprovalCommandHandler(IApprovalWorkflowService workflow)
        => _workflow = workflow;

    public async Task<BaseResponse<ApprovalRequestListItemResponse>> Handle(
        CancelApprovalCommand request, CancellationToken ct)
    {
        try
        {
            var result = await _workflow.CancelAsync(request.Id, request.ActingUserId, request.Note, ct);
            return BaseResponse<ApprovalRequestListItemResponse>.Success(ApprovalRequestMapper.Map(result));
        }
        catch (InvalidOperationException ex)
        {
            return BaseResponse<ApprovalRequestListItemResponse>.Failure(ex.Message);
        }
    }
}
