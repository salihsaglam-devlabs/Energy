using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Commands.SetRolePermissions;

public sealed class SetRolePermissionsCommandHandler
    : IRequestHandler<SetRolePermissionsCommand, BaseResponse<IReadOnlyList<PermissionResponse>>>
{
    private readonly IRoleService _roleService;

    public SetRolePermissionsCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<BaseResponse<IReadOnlyList<PermissionResponse>>> Handle(
        SetRolePermissionsCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _roleService.SetRolePermissionsAsync(request.RoleId, request.PermissionIds, cancellationToken);
        return BaseResponse<IReadOnlyList<PermissionResponse>>.Success(result);
    }
}
