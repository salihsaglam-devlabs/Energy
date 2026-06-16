using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectMember.Commands.DeleteProjectMember;

/// <summary>ProjectMember kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteProjectMemberCommand(Guid Id) : IRequest<BaseResponse<bool>>;
