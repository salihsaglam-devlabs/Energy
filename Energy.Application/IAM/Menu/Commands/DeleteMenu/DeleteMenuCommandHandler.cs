using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Application.System.Services;
using MediatR;

namespace Energy.Application.IAM.Menu.Commands.DeleteMenu;

/// <summary><see cref="DeleteMenuCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class DeleteMenuCommandHandler
    : IRequestHandler<DeleteMenuCommand, BaseResponse<bool>>
{
    private readonly IMenuService _menus;

    public DeleteMenuCommandHandler(IMenuService menus)
    {
        _menus = menus;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteMenuCommand request, CancellationToken ct)
    {
        var result = await _menus.DeleteAsync(request.Id, ct);
        return BaseResponse<bool>.Success(result);
    }
}
