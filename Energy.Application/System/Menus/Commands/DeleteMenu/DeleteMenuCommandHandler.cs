using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Commands.DeleteMenu;

public sealed class DeleteMenuCommandHandler
    : IRequestHandler<DeleteMenuCommand, BaseResponse<Guid>>
{
    private readonly IMenuService _menuService;

    public DeleteMenuCommandHandler(IMenuService menuService)
    {
        _menuService = menuService;
    }

    public async Task<BaseResponse<Guid>> Handle(
        DeleteMenuCommand request,
        CancellationToken cancellationToken)
    {
        await _menuService.DeleteMenuAsync(request.Id, cancellationToken);
        return BaseResponse<Guid>.Success(request.Id);
    }
}
