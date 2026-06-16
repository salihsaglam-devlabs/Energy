using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Requests;
using MediatR;

namespace Energy.Application.Projects.ProjectPhas.Commands.UpdateProjectPhas;

/// <summary>Var olan ProjectPhas kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateProjectPhasCommand(Guid Id, UpdateProjectPhasRequest Request)
    : IRequest<BaseResponse<bool>>;
