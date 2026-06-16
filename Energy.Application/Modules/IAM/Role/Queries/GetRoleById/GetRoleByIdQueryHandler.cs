using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.Role.Queries.GetRoleById;

/// <summary><see cref="GetRoleByIdQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetRoleByIdQueryHandler
    : IRequestHandler<GetRoleByIdQuery, BaseResponse<RoleDetailResponse>>
{
    private readonly IRoleService _roles;

    public GetRoleByIdQueryHandler(IRoleService roles)
    {
        _roles = roles;
    }

    public async Task<BaseResponse<RoleDetailResponse>> Handle(GetRoleByIdQuery request, CancellationToken ct)
    {
        var result = await _roles.GetByIdAsync(request.Id, ct);
        return result is null
            ? BaseResponse<RoleDetailResponse>.Failure("Role not found.")
            : BaseResponse<RoleDetailResponse>.Success(result);
    }
}
