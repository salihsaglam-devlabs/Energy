import os

base = os.path.join(os.path.dirname(__file__), "..", "..",
                    "Energy.Application", "Modules", "Workflow", "Processes", "Approval", "Commands")
base = os.path.abspath(base)
acts = [("Approve", "ApproveAsync", "onaylar (sıradaki adıma ilerletir/tamamlar)"),
        ("Reject", "RejectAsync", "reddeder (kaynak belge Approved olmaz)"),
        ("Cancel", "CancelAsync", "iptal eder")]

for name, method, doc in acts:
    d = os.path.join(base, f"{name}Approval")
    os.makedirs(d, exist_ok=True)
    cmd = f"""using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.Processes.Approval.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.Processes.Approval.Commands.{name}Approval;

/// <summary>Onay talebini {doc} use-case'i.</summary>
/// <param name="Id">Onay talebi kimliği.</param>
/// <param name="ActingUserId">İşlemi yapan kullanıcı kimliği.</param>
/// <param name="Note">Opsiyonel açıklama.</param>
public sealed record {name}ApprovalCommand(Guid Id, Guid ActingUserId, string? Note)
    : IRequest<BaseResponse<ApprovalRequestListItemResponse>>;
"""
    handler = f"""using Energy.Application.Modules.Workflow.Processes.Approval;
using Energy.Application.Workflow.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.Processes.Approval.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.Processes.Approval.Commands.{name}Approval;

/// <summary><see cref="{name}ApprovalCommand"/> handler'ı (orkestrasyon, transaction-güvenli servis).</summary>
public sealed class {name}ApprovalCommandHandler
    : IRequestHandler<{name}ApprovalCommand, BaseResponse<ApprovalRequestListItemResponse>>
{{
    private readonly IApprovalWorkflowService _workflow;

    public {name}ApprovalCommandHandler(IApprovalWorkflowService workflow)
        => _workflow = workflow;

    public async Task<BaseResponse<ApprovalRequestListItemResponse>> Handle(
        {name}ApprovalCommand request, CancellationToken ct)
    {{
        try
        {{
            var result = await _workflow.{method}(request.Id, request.ActingUserId, request.Note, ct);
            return BaseResponse<ApprovalRequestListItemResponse>.Success(ApprovalRequestMapper.Map(result));
        }}
        catch (InvalidOperationException ex)
        {{
            return BaseResponse<ApprovalRequestListItemResponse>.Failure(ex.Message);
        }}
    }}
}}
"""
    with open(os.path.join(d, f"{name}ApprovalCommand.cs"), "w", encoding="utf-8") as f:
        f.write(cmd)
    with open(os.path.join(d, f"{name}ApprovalCommandHandler.cs"), "w", encoding="utf-8") as f:
        f.write(handler)

print("approval commands written")

