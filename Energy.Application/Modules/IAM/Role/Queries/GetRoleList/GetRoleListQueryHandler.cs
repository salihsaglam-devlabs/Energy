using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.Role.Queries.GetRoleList;

/// <summary><see cref="GetRoleListQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetRoleListQueryHandler
    : IRequestHandler<GetRoleListQuery, BaseResponse<PaginatedResponse<RoleSummaryResponse>>>
{
    private readonly IRoleService _roles;

    public GetRoleListQueryHandler(IRoleService roles)
    {
        _roles = roles;
    }

    public async Task<BaseResponse<PaginatedResponse<RoleSummaryResponse>>> Handle(GetRoleListQuery request, CancellationToken ct)
    {
        var result = await _roles.GetAllAsync(request.Request, ct);
        return BaseResponse<PaginatedResponse<RoleSummaryResponse>>.Success(result);
    }
}
