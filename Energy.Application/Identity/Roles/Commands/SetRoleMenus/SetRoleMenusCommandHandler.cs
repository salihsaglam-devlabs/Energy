using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Commands.SetRoleMenus;

public sealed class SetRoleMenusCommandHandler
    : IRequestHandler<SetRoleMenusCommand, BaseResponse<IReadOnlyList<MenuResponse>>>
{
    private readonly IRoleService _roleService;

    public SetRoleMenusCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<BaseResponse<IReadOnlyList<MenuResponse>>> Handle(
        SetRoleMenusCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _roleService.SetRoleMenusAsync(request.RoleId, request.MenuIds, cancellationToken);
        return BaseResponse<IReadOnlyList<MenuResponse>>.Success(result);
    }
}
