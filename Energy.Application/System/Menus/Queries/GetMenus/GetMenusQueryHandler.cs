using Energy.Application.System.Services;
using Energy.Application.Common.Pagination;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Queries.GetMenus;

public sealed class GetMenusQueryHandler
    : IRequestHandler<GetMenusQuery, BaseResponse<PaginatedResponse<MenuResponse>>>
{
    private readonly IMenuService _menuService;

    public GetMenusQueryHandler(IMenuService menuService)
    {
        _menuService = menuService;
    }

    public async Task<BaseResponse<PaginatedResponse<MenuResponse>>> Handle(
        GetMenusQuery request,
        CancellationToken cancellationToken)
    {
        var all = await _menuService.GetMenusAsync(cancellationToken);

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
