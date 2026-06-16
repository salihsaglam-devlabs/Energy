using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectPhas.Commands.DeleteProjectPhas;

/// <summary>ProjectPhas kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteProjectPhasCommand(Guid Id) : IRequest<BaseResponse<bool>>;
