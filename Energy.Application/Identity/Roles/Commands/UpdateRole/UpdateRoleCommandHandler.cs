using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Commands.UpdateRole;

public sealed class UpdateRoleCommandHandler
    : IRequestHandler<UpdateRoleCommand, BaseResponse<RoleDetailResponse>>
{
    private readonly IRoleService _roleService;

    public UpdateRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<BaseResponse<RoleDetailResponse>> Handle(
        UpdateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _roleService.UpdateRoleAsync(request.Id, request.Request, cancellationToken);
        return BaseResponse<RoleDetailResponse>.Success(result);
    }
}
