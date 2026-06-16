using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.IAM.Role.Commands.SetRolePermissions;

/// <summary><see cref="SetRolePermissionsCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class SetRolePermissionsCommandHandler
    : IRequestHandler<SetRolePermissionsCommand, BaseResponse<RoleDetailResponse>>
{
    private readonly IRoleService _roles;

    public SetRolePermissionsCommandHandler(IRoleService roles)
    {
        _roles = roles;
    }

    public async Task<BaseResponse<RoleDetailResponse>> Handle(SetRolePermissionsCommand request, CancellationToken ct)
    {
        var result = await _roles.SetPermissionsAsync(request.Id, request.Request, ct);
        return BaseResponse<RoleDetailResponse>.Success(result);
    }
}
