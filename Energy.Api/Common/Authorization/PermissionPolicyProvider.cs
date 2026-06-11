using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Energy.Api.Common.Authorization;

/// <summary>
/// Dynamically materializes a one-permission policy for any unknown policy name
/// (e.g. "Default.Read"). Falls back to the default provider for built-in policies.
/// </summary>
public sealed class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallback;

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _fallback = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallback.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _fallback.GetFallbackPolicyAsync();

    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var existing = await _fallback.GetPolicyAsync(policyName);
        if (existing is not null)
        {
            return existing;
        }

        // Treat any unknown policy name as a permission claim requirement.
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();

        return policy;
    }
}

