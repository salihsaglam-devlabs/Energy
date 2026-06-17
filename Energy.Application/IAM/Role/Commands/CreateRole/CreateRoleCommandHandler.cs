using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.IAM.Role.Commands.CreateRole;

/// <summary><see cref="CreateRoleCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class CreateRoleCommandHandler
    : IRequestHandler<CreateRoleCommand, BaseResponse<RoleDetailResponse>>
{
    private readonly IRoleService _roles;

    public CreateRoleCommandHandler(IRoleService roles)
    {
        _roles = roles;
    }

    public async Task<BaseResponse<RoleDetailResponse>> Handle(CreateRoleCommand request, CancellationToken ct)
    {
        var result = await _roles.CreateAsync(request.Request, ct);
        return BaseResponse<RoleDetailResponse>.Success(result);
    }
}
