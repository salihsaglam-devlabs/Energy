using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;

namespace Energy.Application.System.Services;

public interface IAccessRuleService
{
    Task<IReadOnlyList<AccessRuleResponse>> GetAccessRulesAsync(CancellationToken cancellationToken = default);

    Task<AccessRuleResponse> GetAccessRuleByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<AccessRuleResponse> CreateAccessRuleAsync(CreateAccessRuleRequest request, CancellationToken cancellationToken = default);

    Task<AccessRuleResponse> UpdateAccessRuleAsync(Guid id, UpdateAccessRuleRequest request, CancellationToken cancellationToken = default);

    Task DeleteAccessRuleAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PermissionResponse>> GetAccessRulePermissionsAsync(Guid accessRuleId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PermissionResponse>> SetAccessRulePermissionsAsync(
        Guid accessRuleId,
        IReadOnlyCollection<Guid> permissionIds,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetRequiredPermissionCodesAsync(
        string scope,
        string path,
        string? httpMethod,
        CancellationToken cancellationToken = default);
}

