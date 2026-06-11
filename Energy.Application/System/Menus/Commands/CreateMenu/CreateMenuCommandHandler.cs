using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Commands.CreateMenu;

public sealed class CreateMenuCommandHandler
    : IRequestHandler<CreateMenuCommand, BaseResponse<MenuResponse>>
{
    private readonly IMenuService _menuService;

    public CreateMenuCommandHandler(IMenuService menuService)
    {
        _menuService = menuService;
    }

    public async Task<BaseResponse<MenuResponse>> Handle(
        CreateMenuCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _menuService.CreateMenuAsync(request.Request, cancellationToken);
        return BaseResponse<MenuResponse>.Success(result);
    }
}
