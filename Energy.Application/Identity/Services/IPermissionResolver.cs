namespace Energy.Application.Identity.Services;

/// <summary>
/// Resolves the effective permission set for a user through the
/// User → Role → Permission chain. Implementations must cache and invalidate
/// per <c>userId</c>.
/// </summary>
public interface IPermissionResolver
{
    Task<IReadOnlySet<string>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<bool> HasPermissionAsync(Guid userId, string permissionCode, CancellationToken cancellationToken = default);

    /// <summary>Drops the cached set for the supplied user.</summary>
    void InvalidateUser(Guid userId);

    /// <summary>Drops cached sets for every user that holds the supplied role.</summary>
    Task InvalidateRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
}
