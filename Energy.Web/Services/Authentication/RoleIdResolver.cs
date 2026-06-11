using Energy.Shared.Models.V1.Common.Requests;
using Energy.Web.Clients.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Energy.Web.Services.Authentication;

/// <summary>
/// Resolves the database identifiers of the roles a freshly authenticated
/// user belongs to. The mapping is cached in-memory because the role catalog
/// changes rarely.
/// </summary>
public interface IRoleIdResolver
{
    Task<IReadOnlyList<Guid>> ResolveAsync(
        IEnumerable<string> roleNames,
        CancellationToken cancellationToken = default);
}

public sealed class RoleIdResolver : IRoleIdResolver
{
    private const string CacheKey = "energy.role-name-id-map";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    private readonly IRoleApiClient _roleApiClient;
    private readonly IMemoryCache _cache;

    public RoleIdResolver(IRoleApiClient roleApiClient, IMemoryCache cache)
    {
        _roleApiClient = roleApiClient;
        _cache = cache;
    }

    public async Task<IReadOnlyList<Guid>> ResolveAsync(
        IEnumerable<string> roleNames,
        CancellationToken cancellationToken = default)
    {
        var names = roleNames
            .Where(n => !string.IsNullOrEmpty(n))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (names.Count == 0)
        {
            return Array.Empty<Guid>();
        }

        var map = await GetRoleMapAsync(cancellationToken);

        return map
            .Where(kv => names.Contains(kv.Key))
            .Select(kv => kv.Value)
            .ToArray();
    }

    private async Task<IReadOnlyDictionary<string, Guid>> GetRoleMapAsync(CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue(CacheKey, out IReadOnlyDictionary<string, Guid>? cached) && cached is not null)
        {
            return cached;
        }

        // Pull a generous page so small/medium catalogs fit in one round-trip.
        var envelope = await _roleApiClient.GetRolesAsync(
            new PaginatedRequest { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        var dict = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);

        if (envelope.IsSuccess && envelope.Data is { Items.Count: > 0 })
        {
            foreach (var role in envelope.Data.Items)
            {
                if (!string.IsNullOrEmpty(role.Name))
                {
                    dict[role.Name] = role.Id;
                }
            }
        }

        _cache.Set(CacheKey, (IReadOnlyDictionary<string, Guid>)dict, CacheDuration);
        return dict;
    }
}

