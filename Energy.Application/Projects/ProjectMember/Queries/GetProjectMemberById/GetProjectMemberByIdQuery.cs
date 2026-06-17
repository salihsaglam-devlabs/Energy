using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectMember.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectMember.Queries.GetProjectMemberById;

/// <summary>Kimliğe göre ProjectMember detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetProjectMemberByIdQuery(Guid Id)
    : IRequest<BaseResponse<ProjectMemberDetailResponse>>;
