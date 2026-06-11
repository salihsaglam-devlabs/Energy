using Energy.Application.Identity.Services;
using Energy.Application.Common.Pagination;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Queries.GetRolePermissions;

public sealed class GetRolePermissionsQueryHandler
    : IRequestHandler<GetRolePermissionsQuery, BaseResponse<PaginatedResponse<PermissionResponse>>>
{
    private readonly IRoleService _roleService;

    public GetRolePermissionsQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<BaseResponse<PaginatedResponse<PermissionResponse>>> Handle(
        GetRolePermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var all = await _roleService.GetRolePermissionsAsync(request.RoleId, cancellationToken);

        var paged = all.ToPaginatedResponse(request,
            searchPredicate: (p, term) =>
                p.Code.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                p.Name.Contains(term, StringComparison.OrdinalIgnoreCase),
            sortSelectors: new Dictionary<string, Func<PermissionResponse, object?>>(StringComparer.OrdinalIgnoreCase)
            {
                ["code"] = p => p.Code,
                ["name"] = p => p.Name
            });

        return BaseResponse<PaginatedResponse<PermissionResponse>>.Success(paged);
    }
}
