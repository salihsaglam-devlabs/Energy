using Energy.Application.Identity.Services;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Identity;
using Energy.Shared.Identity.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Energy.Infrastructure.Identity.Services;

/// <summary>
/// Computes the User → Role → Permission chain once per user and caches the
/// result. SuperAdmin is hard-wired to the full <see cref="PermissionCatalog"/>.
/// </summary>
public sealed class PermissionResolver : IPermissionResolver
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(10);

    private readonly AppDbContext _dbContext;
    private readonly IMemoryCache _cache;

    public PermissionResolver(AppDbContext dbContext, IMemoryCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<IReadOnlySet<string>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(CacheKey(userId), out IReadOnlySet<string>? cached) && cached is not null)
        {
            return cached;
        }

        var roleNames = await _dbContext.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Join(_dbContext.Roles.AsNoTracking(), ur => ur.RoleId, r => r.Id, (_, r) => r.Name)
            .ToListAsync(cancellationToken);

        IReadOnlySet<string> result;
        if (roleNames.Any(name => string.Equals(name, SystemRoles.SuperAdmin, StringComparison.OrdinalIgnoreCase)))
        {
            result = PermissionCatalog.AllCodes;
        }
        else
        {
            var roleCodes = await _dbContext.UserRoles
                .AsNoTracking()
                .Where(ur => ur.UserId == userId)
                .Join(_dbContext.RolePermissions.AsNoTracking(),
                    ur => ur.RoleId, rp => rp.RoleId, (_, rp) => rp.PermissionCode)
                .Distinct()
                .ToListAsync(cancellationToken);

            // Direct, per-user grants are layered on top of the role-derived set.
            var directCodes = await _dbContext.UserPermissions
                .AsNoTracking()
                .Where(up => up.UserId == userId)
                .Select(up => up.PermissionCode)
                .ToListAsync(cancellationToken);

            var set = new HashSet<string>(roleCodes, StringComparer.OrdinalIgnoreCase);
            set.UnionWith(directCodes);
            result = set;
        }

        _cache.Set(CacheKey(userId), result, CacheTtl);
        return result;
    }

    public async Task<bool> HasPermissionAsync(Guid userId, string permissionCode, CancellationToken cancellationToken = default)
    {
        var set = await GetPermissionsAsync(userId, cancellationToken);
        return set.Contains(permissionCode);
    }

    public void InvalidateUser(Guid userId) => _cache.Remove(CacheKey(userId));

    public async Task InvalidateRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        var userIds = await _dbContext.UserRoles
            .AsNoTracking()
            .Where(ur => ur.RoleId == roleId)
            .Select(ur => ur.UserId)
            .ToListAsync(cancellationToken);

        foreach (var id in userIds)
        {
            _cache.Remove(CacheKey(id));
        }
    }

    private static string CacheKey(Guid userId) => $"perm:user:{userId:N}";
}
