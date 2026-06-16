using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.Role.Commands.UpdateRole;

/// <summary><see cref="UpdateRoleCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class UpdateRoleCommandHandler
    : IRequestHandler<UpdateRoleCommand, BaseResponse<RoleDetailResponse>>
{
    private readonly IRoleService _roles;

    public UpdateRoleCommandHandler(IRoleService roles)
    {
        _roles = roles;
    }

    public async Task<BaseResponse<RoleDetailResponse>> Handle(UpdateRoleCommand request, CancellationToken ct)
    {
        var result = await _roles.UpdateAsync(request.Id, request.Request, ct);
        return BaseResponse<RoleDetailResponse>.Success(result);
    }
}
