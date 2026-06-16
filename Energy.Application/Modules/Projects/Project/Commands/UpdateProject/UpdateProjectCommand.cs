using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.Project.Requests;
using MediatR;

namespace Energy.Application.Modules.Projects.Project.Commands.UpdateProject;

/// <summary>Var olan Project kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateProjectCommand(Guid Id, UpdateProjectRequest Request)
    : IRequest<BaseResponse<bool>>;
