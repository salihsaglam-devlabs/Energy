using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Workflow.ApprovalDefinition.Queries.GetApprovalDefinitionById;

/// <summary>Kimliğe göre ApprovalDefinition detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetApprovalDefinitionByIdQuery(Guid Id)
    : IRequest<BaseResponse<ApprovalDefinitionDetailResponse>>;
