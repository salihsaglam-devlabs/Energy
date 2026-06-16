using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Requests;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectLocation.Commands.UpdateProjectLocation;

/// <summary>Var olan ProjectLocation kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateProjectLocationCommand(Guid Id, UpdateProjectLocationRequest Request)
    : IRequest<BaseResponse<bool>>;
