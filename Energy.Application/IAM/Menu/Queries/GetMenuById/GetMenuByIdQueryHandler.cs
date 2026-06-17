using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Application.System.Services;
using MediatR;

namespace Energy.Application.IAM.Menu.Queries.GetMenuById;

/// <summary><see cref="GetMenuByIdQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetMenuByIdQueryHandler
    : IRequestHandler<GetMenuByIdQuery, BaseResponse<MenuResponse>>
{
    private readonly IMenuService _menus;

    public GetMenuByIdQueryHandler(IMenuService menus)
    {
        _menus = menus;
    }

    public async Task<BaseResponse<MenuResponse>> Handle(GetMenuByIdQuery request, CancellationToken ct)
    {
        var result = await _menus.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(LocalizationKeys.Messages.MenuNotFound, request.Id);
        return BaseResponse<MenuResponse>.Success(result);
    }
}
