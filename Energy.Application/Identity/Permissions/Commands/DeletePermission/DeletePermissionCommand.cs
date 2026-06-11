using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Identity.Permissions.Commands.DeletePermission;

public sealed record DeletePermissionCommand(Guid Id) : IRequest<BaseResponse<Guid>>;
