using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Queries.GetMenuById;

public sealed class GetMenuByIdQueryHandler
    : IRequestHandler<GetMenuByIdQuery, BaseResponse<MenuResponse>>
{
    private readonly IMenuService _menuService;

    public GetMenuByIdQueryHandler(IMenuService menuService)
    {
        _menuService = menuService;
    }

    public async Task<BaseResponse<MenuResponse>> Handle(
        GetMenuByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _menuService.GetMenuByIdAsync(request.Id, cancellationToken);
        return BaseResponse<MenuResponse>.Success(result);
    }
}
