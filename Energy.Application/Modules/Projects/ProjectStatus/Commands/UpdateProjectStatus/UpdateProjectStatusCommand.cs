using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Requests;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectStatus.Commands.UpdateProjectStatus;

/// <summary>Var olan ProjectStatus kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateProjectStatusCommand(Guid Id, UpdateProjectStatusRequest Request)
    : IRequest<BaseResponse<bool>>;
