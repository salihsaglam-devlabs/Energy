using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequestApprover.Queries.GetApprovalRequestApproverById;

/// <summary>Kimliğe göre ApprovalRequestApprover detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetApprovalRequestApproverByIdQuery(Guid Id)
    : IRequest<BaseResponse<ApprovalRequestApproverDetailResponse>>;
