using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalCondition.Queries.GetApprovalConditionById;

/// <summary>Kimliğe göre ApprovalCondition detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetApprovalConditionByIdQuery(Guid Id)
    : IRequest<BaseResponse<ApprovalConditionDetailResponse>>;
