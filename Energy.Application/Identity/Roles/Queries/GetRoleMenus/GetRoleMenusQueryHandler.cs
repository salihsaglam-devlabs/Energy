using Energy.Application.Identity.Services;
using Energy.Application.Common.Pagination;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Queries.GetRoleMenus;

public sealed class GetRoleMenusQueryHandler
    : IRequestHandler<GetRoleMenusQuery, BaseResponse<PaginatedResponse<MenuResponse>>>
{
    private readonly IRoleService _roleService;

    public GetRoleMenusQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<BaseResponse<PaginatedResponse<MenuResponse>>> Handle(
        GetRoleMenusQuery request,
        CancellationToken cancellationToken)
    {
        var all = await _roleService.GetRoleMenusAsync(request.RoleId, cancellationToken);

        var paged = all.ToPaginatedResponse(request,
            searchPredicate: (m, term) =>
                m.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                m.Url.Contains(term, StringComparison.OrdinalIgnoreCase),
            sortSelectors: new Dictionary<string, Func<MenuResponse, object?>>(StringComparer.OrdinalIgnoreCase)
            {
                ["name"] = m => m.Name,
                ["order"] = m => m.Order,
                ["url"] = m => m.Url
            });

        return BaseResponse<PaginatedResponse<MenuResponse>>.Success(paged);
    }
}
