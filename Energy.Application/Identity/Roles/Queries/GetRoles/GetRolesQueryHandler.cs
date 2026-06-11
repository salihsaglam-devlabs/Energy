using Energy.Application.Identity.Services;
using Energy.Application.Common.Pagination;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Queries.GetRoles;

public sealed class GetRolesQueryHandler
    : IRequestHandler<GetRolesQuery, BaseResponse<PaginatedResponse<RoleSummaryResponse>>>
{
    private readonly IRoleService _roleService;

    public GetRolesQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<BaseResponse<PaginatedResponse<RoleSummaryResponse>>> Handle(
        GetRolesQuery request,
        CancellationToken cancellationToken)
    {
        var all = await _roleService.GetRolesAsync(cancellationToken);

        var paged = all.ToPaginatedResponse(request,
            searchPredicate: (r, term) =>
                (r.Name?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false) ||
                r.Description.Contains(term, StringComparison.OrdinalIgnoreCase),
            sortSelectors: new Dictionary<string, Func<RoleSummaryResponse, object?>>(StringComparer.OrdinalIgnoreCase)
            {
                ["name"] = r => r.Name,
                ["description"] = r => r.Description
            });

        return BaseResponse<PaginatedResponse<RoleSummaryResponse>>.Success(paged);
    }
}
