using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Application.Identity.Services;
using Energy.Application.System.Services;
using MediatR;

namespace Energy.Application.IAM.Menu.Queries.GetMyMenu;

/// <summary><see cref="GetMyMenuQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetMyMenuQueryHandler
    : IRequestHandler<GetMyMenuQuery, BaseResponse<IReadOnlyList<MenuTreeNodeResponse>>>
{
    private readonly IMenuService _menus;
    private readonly ICurrentUser _currentUser;

    public GetMyMenuQueryHandler(IMenuService menus, ICurrentUser currentUser)
    {
        _menus = menus;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<IReadOnlyList<MenuTreeNodeResponse>>> Handle(GetMyMenuQuery request, CancellationToken ct)
    {
        var result = await _menus.GetTreeForUserAsync(_currentUser.UserId, ct);
        return BaseResponse<IReadOnlyList<MenuTreeNodeResponse>>.Success(result);
    }
}
