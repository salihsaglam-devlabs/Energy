using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.Project.Commands.DeleteProject;

/// <summary>Project kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteProjectCommand(Guid Id) : IRequest<BaseResponse<bool>>;
