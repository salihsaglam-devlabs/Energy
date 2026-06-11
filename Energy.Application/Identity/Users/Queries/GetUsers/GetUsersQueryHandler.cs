using Energy.Application.Identity.Services;
using Energy.Application.Common.Pagination;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Queries.GetUsers;

public sealed class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, BaseResponse<PaginatedResponse<UserSummaryResponse>>>
{
    private readonly IUserService _userService;

    public GetUsersQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<BaseResponse<PaginatedResponse<UserSummaryResponse>>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var all = await _userService.GetUsersAsync(cancellationToken);

        var paged = all.ToPaginatedResponse(request,
            searchPredicate: (u, term) =>
                u.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                u.LastName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                (u.UserName?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (u.Email?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false),
            sortSelectors: new Dictionary<string, Func<UserSummaryResponse, object?>>(StringComparer.OrdinalIgnoreCase)
            {
                ["firstName"] = u => u.FirstName,
                ["lastName"] = u => u.LastName,
                ["userName"] = u => u.UserName,
                ["email"] = u => u.Email,
                ["isActive"] = u => u.IsActive
            });

        return BaseResponse<PaginatedResponse<UserSummaryResponse>>.Success(paged);
    }
}
