using Energy.Application.Common.Exceptions;
using Energy.Application.System.Services;
using Energy.Domain.System;
using Energy.Infrastructure.Persistence;
using Energy.Localization;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Energy.Infrastructure.System.Services;

public sealed class AccessRuleService : IAccessRuleService
{
    private readonly AppDbContext _dbContext;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public AccessRuleService(AppDbContext dbContext, IStringLocalizer<SharedResource> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;
    }

    public async Task<IReadOnlyList<AccessRuleResponse>> GetAccessRulesAsync(CancellationToken cancellationToken = default)
    {
        var rules = await _dbContext.AccessRules
            .AsNoTracking()
            .OrderBy(rule => rule.Scope)
            .ThenBy(rule => rule.Path)
            .ThenBy(rule => rule.HttpMethod)
            .ToListAsync(cancellationToken);
        
        return rules.Select(Map).ToList();
    }

    public async Task<AccessRuleResponse> GetAccessRuleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var rule = await _dbContext.AccessRules
            .AsNoTracking()
            .Where(item => item.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return rule == null
            ? throw new NotFoundException(string.Format(
                _localizer.GetText(LocalizationKeys.Messages.AccessRuleNotFound, "Access rule '{0}' was not found."),
                id))
            : Map(rule);
    }

    public async Task<AccessRuleResponse> CreateAccessRuleAsync(CreateAccessRuleRequest request, CancellationToken cancellationToken = default)
    {
        var normalized = Normalize(request.Scope, request.Path, request.HttpMethod);
        await EnsureUniqueAsync(null, normalized.Scope, normalized.Path, normalized.HttpMethod, cancellationToken);

        var rule = new AccessRule
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Scope = normalized.Scope,
            Path = normalized.Path,
            HttpMethod = normalized.HttpMethod,
            Description = (request.Description ?? string.Empty).Trim(),
            IsEnabled = request.IsEnabled,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.AccessRules.Add(rule);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetAccessRuleByIdAsync(rule.Id, cancellationToken);
    }

    public async Task<AccessRuleResponse> UpdateAccessRuleAsync(Guid id, UpdateAccessRuleRequest request, CancellationToken cancellationToken = default)
    {
        var rule = await _dbContext.AccessRules.FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
                   ?? throw new NotFoundException(string.Format(
                       _localizer.GetText(LocalizationKeys.Messages.AccessRuleNotFound, "Access rule '{0}' was not found."),
                       id));

        var normalized = Normalize(request.Scope, request.Path, request.HttpMethod);
        await EnsureUniqueAsync(id, normalized.Scope, normalized.Path, normalized.HttpMethod, cancellationToken);

        rule.Name = request.Name.Trim();
        rule.Scope = normalized.Scope;
        rule.Path = normalized.Path;
        rule.HttpMethod = normalized.HttpMethod;
        rule.Description = (request.Description ?? string.Empty).Trim();
        rule.IsEnabled = request.IsEnabled;
        rule.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return await GetAccessRuleByIdAsync(id, cancellationToken);
    }

    public async Task DeleteAccessRuleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var rule = await _dbContext.AccessRules.FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
                   ?? throw new NotFoundException(string.Format(
                       _localizer.GetText(LocalizationKeys.Messages.AccessRuleNotFound, "Access rule '{0}' was not found."),
                       id));

        _dbContext.AccessRules.Remove(rule);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PermissionResponse>> GetAccessRulePermissionsAsync(
        Guid accessRuleId,
        CancellationToken cancellationToken = default)
    {
        await EnsureRuleExistsAsync(accessRuleId, cancellationToken);

        return await _dbContext.AccessRulePermissions
            .AsNoTracking()
            .Where(link => link.AccessRuleId == accessRuleId)
            .Join(_dbContext.Permissions.AsNoTracking(),
                link => link.PermissionId,
                permission => permission.Id,
                (link, permission) => new PermissionResponse
                {
                    Id = permission.Id,
                    Code = permission.Code,
                    Name = permission.Name
                })
            .OrderBy(permission => permission.Code)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PermissionResponse>> SetAccessRulePermissionsAsync(
        Guid accessRuleId,
        IReadOnlyCollection<Guid> permissionIds,
        CancellationToken cancellationToken = default)
    {
        await EnsureRuleExistsAsync(accessRuleId, cancellationToken);

        var distinctIds = permissionIds.Distinct().ToArray();
        if (distinctIds.Length > 0)
        {
            var existingCount = await _dbContext.Permissions
                .AsNoTracking()
                .CountAsync(permission => distinctIds.Contains(permission.Id), cancellationToken);

            if (existingCount != distinctIds.Length)
            {
                throw new NotFoundException(_localizer.GetText(
                    LocalizationKeys.Messages.PermissionsNotFound,
                    "One or more permissions were not found."));
            }
        }

        var existingLinks = await _dbContext.AccessRulePermissions
            .Where(link => link.AccessRuleId == accessRuleId)
            .ToListAsync(cancellationToken);

        var toRemove = existingLinks.Where(link => !distinctIds.Contains(link.PermissionId)).ToList();
        var existingIds = existingLinks.Select(link => link.PermissionId).ToHashSet();
        var toAdd = distinctIds.Where(id => !existingIds.Contains(id))
            .Select(id => new AccessRulePermission { AccessRuleId = accessRuleId, PermissionId = id });

        if (toRemove.Count > 0)
        {
            _dbContext.AccessRulePermissions.RemoveRange(toRemove);
        }

        await _dbContext.AccessRulePermissions.AddRangeAsync(toAdd, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetAccessRulePermissionsAsync(accessRuleId, cancellationToken);
    }

    public async Task<IReadOnlyList<string>> GetRequiredPermissionCodesAsync(
        string scope,
        string path,
        string? httpMethod,
        CancellationToken cancellationToken = default)
    {
        var normalized = Normalize(scope, path, httpMethod);

        var enabledRules = await _dbContext.AccessRules
            .AsNoTracking()
            .Where(rule => rule.IsEnabled && rule.Scope == normalized.Scope)
            .Select(rule => new { rule.Id, rule.Path, rule.HttpMethod })
            .ToListAsync(cancellationToken);

        if (enabledRules.Count == 0)
        {
            return Array.Empty<string>();
        }

        var matchingRuleIds = enabledRules
            .Where(rule => MatchesPathPattern(rule.Path, normalized.Path) &&
                           (string.IsNullOrEmpty(rule.HttpMethod) || rule.HttpMethod == normalized.HttpMethod))
            .Select(rule => rule.Id)
            .ToArray();

        if (matchingRuleIds.Length == 0)
        {
            return Array.Empty<string>();
        }

        return await _dbContext.AccessRulePermissions
            .AsNoTracking()
            .Where(link => matchingRuleIds.Contains(link.AccessRuleId))
            .Join(_dbContext.Permissions.AsNoTracking(),
                link => link.PermissionId,
                permission => permission.Id,
                (link, permission) => permission.Code)
            .Distinct()
            .OrderBy(code => code)
            .ToListAsync(cancellationToken);
    }

    private async Task EnsureRuleExistsAsync(Guid accessRuleId, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.AccessRules
            .AsNoTracking()
            .AnyAsync(rule => rule.Id == accessRuleId, cancellationToken);

        if (!exists)
        {
            throw new NotFoundException(string.Format(
                _localizer.GetText(LocalizationKeys.Messages.AccessRuleNotFound, "Access rule '{0}' was not found."),
                accessRuleId));
        }
    }

    private async Task EnsureUniqueAsync(
        Guid? currentId,
        string scope,
        string path,
        string httpMethod,
        CancellationToken cancellationToken)
    {
        var exists = await _dbContext.AccessRules.AnyAsync(
            rule =>
                rule.Id != currentId &&
                rule.Scope == scope &&
                rule.Path == path &&
                rule.HttpMethod == httpMethod,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(_localizer.GetText(
                LocalizationKeys.Messages.AccessRuleAlreadyExists,
                "An access rule already exists for this scope/path/method combination."));
        }
    }

    private static AccessRuleResponse Map(AccessRule rule) => new()
    {
        Id = rule.Id,
        Name = rule.Name,
        Scope = rule.Scope,
        Path = rule.Path,
        HttpMethod = rule.HttpMethod,
        Description = rule.Description,
        IsEnabled = rule.IsEnabled
    };

    private (string Scope, string Path, string HttpMethod) Normalize(string scope, string path, string? httpMethod)
    {
        if (string.IsNullOrWhiteSpace(scope))
        {
            throw new ConflictException(_localizer.GetText(
                LocalizationKeys.Messages.ScopeRequired,
                "Scope cannot be empty."));
        }

        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ConflictException(_localizer.GetText(
                LocalizationKeys.Messages.PathRequired,
                "Path cannot be empty."));
        }

        var normalizedPath = path.Trim();
        if (!normalizedPath.StartsWith('/'))
        {
            normalizedPath = "/" + normalizedPath;
        }

        if (normalizedPath.Length > 1)
        {
            normalizedPath = normalizedPath.TrimEnd('/');
        }

        return (
            Scope: scope.Trim().ToUpperInvariant(),
            Path: normalizedPath,
            HttpMethod: string.IsNullOrWhiteSpace(httpMethod)
                ? string.Empty
                : httpMethod.Trim().ToUpperInvariant());
    }

    /// <summary>
    /// Matches a request path against a rule pattern.
    /// Supports exact matches, wildcards (*), and parametrized segments ({paramName}).
    /// Examples:
    ///   /system/users -> exact match
    ///   /system/users/* -> matches /system/users/123, /system/users/abc
    ///   /system/users/{id} -> matches /system/users/123, /system/users/abc
    ///   /system/* -> matches /system/anything/nested
    /// </summary>
    private static bool MatchesPathPattern(string pattern, string requestPath)
    {
        // Exact match
        if (pattern == requestPath)
        {
            return true;
        }

        // If pattern doesn't contain wildcards or parameters, no match
        if (!pattern.Contains('*') && !pattern.Contains('{'))
        {
            return false;
        }

        // Convert pattern to regex
        var pathSegments = pattern.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var requestSegments = requestPath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        return MatchSegments(pathSegments, requestSegments, 0, 0);
    }

    private static bool MatchSegments(string[] patternSegments, string[] requestSegments, int patternIndex, int requestIndex)
    {
        // Both exhausted - perfect match
        if (patternIndex >= patternSegments.Length && requestIndex >= requestSegments.Length)
        {
            return true;
        }

        // Pattern exhausted but request has more - no match
        if (patternIndex >= patternSegments.Length)
        {
            return false;
        }

        var patternSegment = patternSegments[patternIndex];

        // Wildcard catches remaining segments
        if (patternSegment == "*")
        {
            return true;
        }

        // Parametrized segment {paramName} matches any single segment
        if (patternSegment.StartsWith('{') && patternSegment.EndsWith('}'))
        {
            if (requestIndex >= requestSegments.Length)
            {
                return false;
            }

            return MatchSegments(patternSegments, requestSegments, patternIndex + 1, requestIndex + 1);
        }

        // Exact segment match
        if (requestIndex < requestSegments.Length && patternSegment == requestSegments[requestIndex])
        {
            return MatchSegments(patternSegments, requestSegments, patternIndex + 1, requestIndex + 1);
        }

        return false;
    }
}

