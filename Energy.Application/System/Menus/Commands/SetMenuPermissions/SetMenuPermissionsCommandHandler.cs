using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Commands.SetMenuPermissions;

public sealed class SetMenuPermissionsCommandHandler
    : IRequestHandler<SetMenuPermissionsCommand, BaseResponse<IReadOnlyList<PermissionResponse>>>
{
    private readonly IMenuService _menuService;

    public SetMenuPermissionsCommandHandler(IMenuService menuService)
    {
        _menuService = menuService;
    }

    public async Task<BaseResponse<IReadOnlyList<PermissionResponse>>> Handle(
        SetMenuPermissionsCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _menuService.SetMenuPermissionsAsync(request.MenuId, request.PermissionIds, cancellationToken);
        return BaseResponse<IReadOnlyList<PermissionResponse>>.Success(result);
    }
}

