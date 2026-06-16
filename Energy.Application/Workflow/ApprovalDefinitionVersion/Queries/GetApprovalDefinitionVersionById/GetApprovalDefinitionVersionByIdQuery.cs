using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalDefinitionVersion.Queries.GetApprovalDefinitionVersionById;

/// <summary>Kimliğe göre ApprovalDefinitionVersion detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetApprovalDefinitionVersionByIdQuery(Guid Id)
    : IRequest<BaseResponse<ApprovalDefinitionVersionDetailResponse>>;
