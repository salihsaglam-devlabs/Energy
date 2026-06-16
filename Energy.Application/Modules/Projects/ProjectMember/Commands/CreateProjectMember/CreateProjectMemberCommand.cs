using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectMember.Requests;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectMember.Commands.CreateProjectMember;

/// <summary>Yeni ProjectMember oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateProjectMemberCommand(CreateProjectMemberRequest Request)
    : IRequest<BaseResponse<Guid>>;
