using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;

namespace Energy.Web.Clients.System;

public interface IAccessRuleApiClient
{
    Task<BaseResponse<PaginatedResponse<AccessRuleResponse>>> GetAccessRulesAsync(
        PaginatedRequest? request = null,
        CancellationToken cancellationToken = default);

    Task<BaseResponse<AccessRuleResponse>> GetAccessRuleAsync(Guid id, CancellationToken cancellationToken = default);

    Task<BaseResponse<AccessRuleResponse>> CreateAccessRuleAsync(CreateAccessRuleRequest request, CancellationToken cancellationToken = default);

    Task<BaseResponse<AccessRuleResponse>> UpdateAccessRuleAsync(Guid id, UpdateAccessRuleRequest request, CancellationToken cancellationToken = default);

    Task<BaseResponse<Guid>> DeleteAccessRuleAsync(Guid id, CancellationToken cancellationToken = default);

    Task<BaseResponse<PaginatedResponse<PermissionResponse>>> GetAccessRulePermissionsAsync(
        Guid id,
        PaginatedRequest? request = null,
        CancellationToken cancellationToken = default);

    Task<BaseResponse<IReadOnlyList<PermissionResponse>>> SetAccessRulePermissionsAsync(
        Guid id,
        SetAccessRulePermissionsRequest request,
        CancellationToken cancellationToken = default);

    Task<BaseResponse<IReadOnlyList<string>>> GetRequiredPermissionsAsync(
        string scope,
        string path,
        string? httpMethod = null,
        CancellationToken cancellationToken = default);
}

