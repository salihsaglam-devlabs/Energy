using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalAction.Queries.GetApprovalActionById;

/// <summary>Kimliğe göre ApprovalAction detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetApprovalActionByIdQuery(Guid Id)
    : IRequest<BaseResponse<ApprovalActionDetailResponse>>;
