using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Commands.SetMenuPermissions;

public sealed record SetMenuPermissionsCommand(Guid MenuId, IReadOnlyCollection<Guid> PermissionIds)
    : IRequest<BaseResponse<IReadOnlyList<PermissionResponse>>>;

