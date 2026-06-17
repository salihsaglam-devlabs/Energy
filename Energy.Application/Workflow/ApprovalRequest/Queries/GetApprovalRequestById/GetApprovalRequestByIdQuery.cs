using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequest.Queries.GetApprovalRequestById;

/// <summary>Kimliğe göre ApprovalRequest detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetApprovalRequestByIdQuery(Guid Id)
    : IRequest<BaseResponse<ApprovalRequestDetailResponse>>;
