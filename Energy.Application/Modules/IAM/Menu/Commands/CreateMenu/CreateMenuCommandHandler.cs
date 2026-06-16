using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Application.System.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.Menu.Commands.CreateMenu;

/// <summary><see cref="CreateMenuCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class CreateMenuCommandHandler
    : IRequestHandler<CreateMenuCommand, BaseResponse<MenuResponse>>
{
    private readonly IMenuService _menus;

    public CreateMenuCommandHandler(IMenuService menus)
    {
        _menus = menus;
    }

    public async Task<BaseResponse<MenuResponse>> Handle(CreateMenuCommand request, CancellationToken ct)
    {
        var result = await _menus.CreateAsync(request.Request, ct);
        return BaseResponse<MenuResponse>.Success(result);
    }
}
