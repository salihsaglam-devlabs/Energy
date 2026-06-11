using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Commands.CreateRole;

public sealed class CreateRoleCommandHandler
    : IRequestHandler<CreateRoleCommand, BaseResponse<RoleDetailResponse>>
{
    private readonly IRoleService _roleService;

    public CreateRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<BaseResponse<RoleDetailResponse>> Handle(
        CreateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _roleService.CreateRoleAsync(request.Request, cancellationToken);
        return BaseResponse<RoleDetailResponse>.Success(result);
    }
}
