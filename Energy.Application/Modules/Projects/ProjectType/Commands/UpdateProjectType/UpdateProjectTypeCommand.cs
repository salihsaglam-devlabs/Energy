using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectType.Requests;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectType.Commands.UpdateProjectType;

/// <summary>Var olan ProjectType kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateProjectTypeCommand(Guid Id, UpdateProjectTypeRequest Request)
    : IRequest<BaseResponse<bool>>;
