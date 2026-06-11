using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Commands.SetRolePermissions;

public sealed record SetRolePermissionsCommand(Guid RoleId, IReadOnlyCollection<Guid> PermissionIds)
    : IRequest<BaseResponse<IReadOnlyList<PermissionResponse>>>;
