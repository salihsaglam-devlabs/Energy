using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.Processes.Approval.Responses;
using MediatR;
namespace Energy.Application.Workflow.Processes.Approval.Queries.GetMyPendingApprovals;
/// <summary>Oturum sahibinin bekleyen onay taleplerini getiren sorgu.</summary>
/// <param name="UserId">Bekleyen onayları istenen kullanıcı kimliği.</param>
public sealed record GetMyPendingApprovalsQuery(Guid UserId)
    : IRequest<BaseResponse<IReadOnlyList<ApprovalRequestListItemResponse>>>;
