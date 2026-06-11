using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.System;

public sealed class AccessRuleApiClient : ApiClientBase, IAccessRuleApiClient
{
    public AccessRuleApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<BaseResponse<PaginatedResponse<AccessRuleResponse>>> GetAccessRulesAsync(
        PaginatedRequest? request = null,
        CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<PaginatedResponse<AccessRuleResponse>>>(
            ApiQueryString.Append(ApiRoutes.AccessRules.Base, request),
            cancellationToken);

    public Task<BaseResponse<AccessRuleResponse>> GetAccessRuleAsync(Guid id, CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<AccessRuleResponse>>(ApiRoutes.AccessRules.ById(id), cancellationToken);

    public Task<BaseResponse<AccessRuleResponse>> CreateAccessRuleAsync(
        CreateAccessRuleRequest request,
        CancellationToken cancellationToken = default)
        => PostAsync<CreateAccessRuleRequest, BaseResponse<AccessRuleResponse>>(ApiRoutes.AccessRules.Base, request, cancellationToken);

    public Task<BaseResponse<AccessRuleResponse>> UpdateAccessRuleAsync(
        Guid id,
        UpdateAccessRuleRequest request,
        CancellationToken cancellationToken = default)
        => PutAsync<UpdateAccessRuleRequest, BaseResponse<AccessRuleResponse>>(ApiRoutes.AccessRules.ById(id), request, cancellationToken);

    public Task<BaseResponse<Guid>> DeleteAccessRuleAsync(Guid id, CancellationToken cancellationToken = default)
        => DeleteAsync<BaseResponse<Guid>>(ApiRoutes.AccessRules.ById(id), cancellationToken);

    public Task<BaseResponse<PaginatedResponse<PermissionResponse>>> GetAccessRulePermissionsAsync(
        Guid id,
        PaginatedRequest? request = null,
        CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<PaginatedResponse<PermissionResponse>>>(
            ApiQueryString.Append(ApiRoutes.AccessRules.Permissions(id), request),
            cancellationToken);

    public Task<BaseResponse<IReadOnlyList<PermissionResponse>>> SetAccessRulePermissionsAsync(
        Guid id,
        SetAccessRulePermissionsRequest request,
        CancellationToken cancellationToken = default)
        => PutAsync<SetAccessRulePermissionsRequest, BaseResponse<IReadOnlyList<PermissionResponse>>>(
            ApiRoutes.AccessRules.Permissions(id),
            request,
            cancellationToken);

    public Task<BaseResponse<IReadOnlyList<string>>> GetRequiredPermissionsAsync(
        string scope,
        string path,
        string? httpMethod = null,
        CancellationToken cancellationToken = default)
    {
        var query = ApiQueryString.Append(
            ApiRoutes.AccessRules.RequiredPermissions,
            ("scope", scope),
            ("path", path),
            ("httpMethod", httpMethod));

        return GetAsync<BaseResponse<IReadOnlyList<string>>>(query, cancellationToken);
    }
}

