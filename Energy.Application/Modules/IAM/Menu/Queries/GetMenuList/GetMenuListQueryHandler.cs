using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Application.System.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.Menu.Queries.GetMenuList;

/// <summary><see cref="GetMenuListQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetMenuListQueryHandler
    : IRequestHandler<GetMenuListQuery, BaseResponse<PaginatedResponse<MenuResponse>>>
{
    private readonly IMenuService _menus;

    public GetMenuListQueryHandler(IMenuService menus)
    {
        _menus = menus;
    }

    public async Task<BaseResponse<PaginatedResponse<MenuResponse>>> Handle(GetMenuListQuery request, CancellationToken ct)
    {
        var result = await _menus.GetAllAsync(request.Request, ct);
        return BaseResponse<PaginatedResponse<MenuResponse>>.Success(result);
    }
}
