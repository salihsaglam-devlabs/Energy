using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Commands.SeedDefaultMenus;

public sealed class SeedDefaultMenusCommandHandler
    : IRequestHandler<SeedDefaultMenusCommand, BaseResponse<SeedResultResponse>>
{
    private readonly IMenuService _menuService;

    public SeedDefaultMenusCommandHandler(IMenuService menuService)
    {
        _menuService = menuService;
    }

    public async Task<BaseResponse<SeedResultResponse>> Handle(
        SeedDefaultMenusCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _menuService.SeedDefaultMenusAsync(cancellationToken);
        return BaseResponse<SeedResultResponse>.Success(result);
    }
}
