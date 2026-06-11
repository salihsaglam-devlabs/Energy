using Energy.Application.Common.Pagination;
using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Queries.GetMenuPermissions;

public sealed class GetMenuPermissionsQueryHandler
    : IRequestHandler<GetMenuPermissionsQuery, BaseResponse<PaginatedResponse<PermissionResponse>>>
{
    private readonly IMenuService _menuService;

    public GetMenuPermissionsQueryHandler(IMenuService menuService)
    {
        _menuService = menuService;
    }

    public async Task<BaseResponse<PaginatedResponse<PermissionResponse>>> Handle(
        GetMenuPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var all = await _menuService.GetMenuPermissionsAsync(request.MenuId, cancellationToken);

        var paged = all.ToPaginatedResponse(request,
            searchPredicate: (permission, term) =>
                permission.Code.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                permission.Name.Contains(term, StringComparison.OrdinalIgnoreCase),
            sortSelectors: new Dictionary<string, Func<PermissionResponse, object?>>(StringComparer.OrdinalIgnoreCase)
            {
                ["code"] = permission => permission.Code,
                ["name"] = permission => permission.Name
            });

        return BaseResponse<PaginatedResponse<PermissionResponse>>.Success(paged);
    }
}

