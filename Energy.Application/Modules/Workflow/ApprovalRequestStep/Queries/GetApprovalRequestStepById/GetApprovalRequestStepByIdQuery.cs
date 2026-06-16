using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalRequestStep.Queries.GetApprovalRequestStepById;

/// <summary>Kimliğe göre ApprovalRequestStep detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetApprovalRequestStepByIdQuery(Guid Id)
    : IRequest<BaseResponse<ApprovalRequestStepDetailResponse>>;
