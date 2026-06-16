using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Application.System.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.Menu.Commands.UpdateMenu;

/// <summary><see cref="UpdateMenuCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class UpdateMenuCommandHandler
    : IRequestHandler<UpdateMenuCommand, BaseResponse<MenuResponse>>
{
    private readonly IMenuService _menus;

    public UpdateMenuCommandHandler(IMenuService menus)
    {
        _menus = menus;
    }

    public async Task<BaseResponse<MenuResponse>> Handle(UpdateMenuCommand request, CancellationToken ct)
    {
        var result = await _menus.UpdateAsync(request.Id, request.Request, ct);
        return BaseResponse<MenuResponse>.Success(result);
    }
}
