using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Queries.GetMenuTree;

public sealed class GetMenuTreeQueryHandler
    : IRequestHandler<GetMenuTreeQuery, BaseResponse<IReadOnlyList<MenuResponse>>>
{
    private readonly IMenuService _menuService;

    public GetMenuTreeQueryHandler(IMenuService menuService)
    {
        _menuService = menuService;
    }

    public async Task<BaseResponse<IReadOnlyList<MenuResponse>>> Handle(
        GetMenuTreeQuery request,
        CancellationToken cancellationToken)
    {
        var tree = await _menuService.GetMenuTreeAsync(cancellationToken);
        return BaseResponse<IReadOnlyList<MenuResponse>>.Success(tree);
    }
}

