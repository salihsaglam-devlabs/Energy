using Energy.Application.Identity.Services;
using Energy.Application.Common.Pagination;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Permissions.Queries.GetPermissions;

public sealed class GetPermissionsQueryHandler
    : IRequestHandler<GetPermissionsQuery, BaseResponse<PaginatedResponse<PermissionResponse>>>
{
    private readonly IPermissionService _permissionService;

    public GetPermissionsQueryHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task<BaseResponse<PaginatedResponse<PermissionResponse>>> Handle(
        GetPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var all = await _permissionService.GetPermissionsAsync(cancellationToken);

        var paged = all.ToPaginatedResponse(request,
            searchPredicate: (p, term) =>
                (p.Code?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.Name?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false),
            sortSelectors: new Dictionary<string, Func<PermissionResponse, object?>>(StringComparer.OrdinalIgnoreCase)
            {
                ["code"] = p => p.Code,
                ["name"] = p => p.Name
            });

        return BaseResponse<PaginatedResponse<PermissionResponse>>.Success(paged);
    }
}
