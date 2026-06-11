using Energy.Application.Common.Pagination;
using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.AccessRules.Queries.GetAccessRules;

public sealed class GetAccessRulesQueryHandler
    : IRequestHandler<GetAccessRulesQuery, BaseResponse<PaginatedResponse<AccessRuleResponse>>>
{
    private readonly IAccessRuleService _accessRuleService;

    public GetAccessRulesQueryHandler(IAccessRuleService accessRuleService)
    {
        _accessRuleService = accessRuleService;
    }

    public async Task<BaseResponse<PaginatedResponse<AccessRuleResponse>>> Handle(
        GetAccessRulesQuery request,
        CancellationToken cancellationToken)
    {
        var all = await _accessRuleService.GetAccessRulesAsync(cancellationToken);

        var paged = all.ToPaginatedResponse(request,
            searchPredicate: (item, term) =>
                item.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                item.Path.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                item.Scope.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                item.Description.Contains(term, StringComparison.OrdinalIgnoreCase),
            sortSelectors: new Dictionary<string, Func<AccessRuleResponse, object?>>(StringComparer.OrdinalIgnoreCase)
            {
                ["name"] = item => item.Name,
                ["scope"] = item => item.Scope,
                ["path"] = item => item.Path,
                ["httpMethod"] = item => item.HttpMethod,
                ["isEnabled"] = item => item.IsEnabled
            });

        return BaseResponse<PaginatedResponse<AccessRuleResponse>>.Success(paged);
    }
}

