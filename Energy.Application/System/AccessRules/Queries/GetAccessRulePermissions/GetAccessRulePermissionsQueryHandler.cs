using Energy.Application.Common.Pagination;
using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.System.AccessRules.Queries.GetAccessRulePermissions;

public sealed class GetAccessRulePermissionsQueryHandler
    : IRequestHandler<GetAccessRulePermissionsQuery, BaseResponse<PaginatedResponse<PermissionResponse>>>
{
    private readonly IAccessRuleService _accessRuleService;

    public GetAccessRulePermissionsQueryHandler(IAccessRuleService accessRuleService)
    {
        _accessRuleService = accessRuleService;
    }

    public async Task<BaseResponse<PaginatedResponse<PermissionResponse>>> Handle(
        GetAccessRulePermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var all = await _accessRuleService.GetAccessRulePermissionsAsync(request.AccessRuleId, cancellationToken);

        var paged = all.ToPaginatedResponse(request,
            searchPredicate: (permission, term) =>
                permission.Code.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                permission.Name.Contains(term, StringComparison.OrdinalIgnoreCase),
            sortSelectors: new Dictionary<string, Func<PermissionResponse, object?>>(StringComparer.OrdinalIgnoreCase)
            {
                ["code"] = permission => permission.Code,
                ["name"] = permission => permission.Name
            });

        return BaseResponse<PaginatedResponse<PermissionResponse>>.Success(paged);
    }
}

