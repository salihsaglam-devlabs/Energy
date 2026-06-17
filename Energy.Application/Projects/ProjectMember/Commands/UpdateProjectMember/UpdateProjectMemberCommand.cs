using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectMember.Requests;
using MediatR;

namespace Energy.Application.Projects.ProjectMember.Commands.UpdateProjectMember;

/// <summary>Var olan ProjectMember kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateProjectMemberCommand(Guid Id, UpdateProjectMemberRequest Request)
    : IRequest<BaseResponse<bool>>;
