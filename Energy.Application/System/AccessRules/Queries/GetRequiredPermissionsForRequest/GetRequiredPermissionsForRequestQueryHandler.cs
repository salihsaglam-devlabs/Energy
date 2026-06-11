using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.System.AccessRules.Queries.GetRequiredPermissionsForRequest;

public sealed class GetRequiredPermissionsForRequestQueryHandler
    : IRequestHandler<GetRequiredPermissionsForRequestQuery, BaseResponse<IReadOnlyList<string>>>
{
    private readonly IAccessRuleService _accessRuleService;

    public GetRequiredPermissionsForRequestQueryHandler(IAccessRuleService accessRuleService)
    {
        _accessRuleService = accessRuleService;
    }

    public async Task<BaseResponse<IReadOnlyList<string>>> Handle(
        GetRequiredPermissionsForRequestQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _accessRuleService.GetRequiredPermissionCodesAsync(
            request.Scope,
            request.Path,
            request.HttpMethod,
            cancellationToken);

        return BaseResponse<IReadOnlyList<string>>.Success(result);
    }
}

