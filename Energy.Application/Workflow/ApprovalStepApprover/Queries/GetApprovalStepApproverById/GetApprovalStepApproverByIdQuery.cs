using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalStepApprover.Queries.GetApprovalStepApproverById;

/// <summary>Kimliğe göre ApprovalStepApprover detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetApprovalStepApproverByIdQuery(Guid Id)
    : IRequest<BaseResponse<ApprovalStepApproverDetailResponse>>;
