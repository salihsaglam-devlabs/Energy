using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectLocation.Commands.DeleteProjectLocation;

/// <summary>ProjectLocation kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteProjectLocationCommand(Guid Id) : IRequest<BaseResponse<bool>>;
