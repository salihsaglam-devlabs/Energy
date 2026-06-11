using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Commands.UpdateMenu;

public sealed class UpdateMenuCommandHandler
    : IRequestHandler<UpdateMenuCommand, BaseResponse<MenuResponse>>
{
    private readonly IMenuService _menuService;

    public UpdateMenuCommandHandler(IMenuService menuService)
    {
        _menuService = menuService;
    }

    public async Task<BaseResponse<MenuResponse>> Handle(
        UpdateMenuCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _menuService.UpdateMenuAsync(request.Id, request.Request, cancellationToken);
        return BaseResponse<MenuResponse>.Success(result);
    }
}
