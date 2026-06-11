using Microsoft.AspNetCore.Authorization;

namespace Energy.Api.Common.Authorization;

/// <summary>
/// Authorization requirement satisfied when the authenticated principal has
/// a <c>permission</c> claim whose value matches <see cref="Permission"/>.
/// </summary>
public sealed class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }

    public string Permission { get; }
}

