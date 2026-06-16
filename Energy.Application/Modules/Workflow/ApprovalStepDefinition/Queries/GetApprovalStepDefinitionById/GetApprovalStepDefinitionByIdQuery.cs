using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalStepDefinition.Queries.GetApprovalStepDefinitionById;

/// <summary>Kimliğe göre ApprovalStepDefinition detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetApprovalStepDefinitionByIdQuery(Guid Id)
    : IRequest<BaseResponse<ApprovalStepDefinitionDetailResponse>>;
