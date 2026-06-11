using Energy.Application.Common.Exceptions;
using Energy.Application.System.Services;
using Energy.Domain.System;
using Energy.Infrastructure.Persistence;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Energy.Infrastructure.System.Services;

public sealed class ApiEndpointService : IApiEndpointService
{
    private const string CacheKey = "endpoints:all";
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    private readonly AppDbContext _db;
    private readonly IMemoryCache _cache;

    public ApiEndpointService(AppDbContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<PaginatedResponse<ApiEndpointResponse>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default)
    {
        var query = _db.ApiEndpoints.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(e => e.Name.ToLower().Contains(term) || e.Path.ToLower().Contains(term));
        }
        var total = await query.CountAsync(ct);
        var page = await query
            .OrderBy(e => e.Path).ThenBy(e => e.HttpMethod)
            .Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
            .Select(e => Project(e))
            .ToListAsync(ct);
        return PaginatedResponse<ApiEndpointResponse>.Create(page, request.PageNumber, request.PageSize, total);
    }

    public async Task<ApiEndpointResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _db.ApiEndpoints.AsNoTracking().Where(e => e.Id == id).Select(e => Project(e)).FirstOrDefaultAsync(ct);
    }

    public async Task<ApiEndpointResponse> CreateAsync(CreateApiEndpointRequest request, CancellationToken ct = default)
    {
        var path = request.Path.Trim();
        var method = request.HttpMethod.Trim().ToUpperInvariant();
        if (await _db.ApiEndpoints.AnyAsync(e => e.HttpMethod == method && e.Path == path, ct))
            throw new ConflictException(LocalizationKeys.Messages.EndpointAlreadyExists, method, path);

        var endpoint = new ApiEndpoint
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Description = request.Description,
            Path = path,
            HttpMethod = method,
            IsActive = request.IsActive,
            RequiredPermissionCode = string.IsNullOrWhiteSpace(request.RequiredPermissionCode) ? null : request.RequiredPermissionCode.Trim()
        };
        _db.ApiEndpoints.Add(endpoint);
        await _db.SaveChangesAsync(ct);
        InvalidateCache();
        return Project(endpoint);
    }

    public async Task<ApiEndpointResponse> UpdateAsync(Guid id, UpdateApiEndpointRequest request, CancellationToken ct = default)
    {
        var endpoint = await _db.ApiEndpoints.FirstOrDefaultAsync(e => e.Id == id, ct)
                       ?? throw new NotFoundException(LocalizationKeys.Messages.EndpointNotFound, id);

        var path = request.Path.Trim();
        var method = request.HttpMethod.Trim().ToUpperInvariant();

        if ((endpoint.Path != path || endpoint.HttpMethod != method) &&
            await _db.ApiEndpoints.AnyAsync(e => e.HttpMethod == method && e.Path == path && e.Id != id, ct))
        {
            throw new ConflictException(LocalizationKeys.Messages.EndpointAlreadyExists, method, path);
        }

        endpoint.Name = request.Name.Trim();
        endpoint.Description = request.Description;
        endpoint.Path = path;
        endpoint.HttpMethod = method;
        endpoint.IsActive = request.IsActive;
        endpoint.RequiredPermissionCode = string.IsNullOrWhiteSpace(request.RequiredPermissionCode) ? null : request.RequiredPermissionCode.Trim();
        await _db.SaveChangesAsync(ct);
        InvalidateCache();
        return Project(endpoint);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var endpoint = await _db.ApiEndpoints.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (endpoint is null) return false;
        _db.ApiEndpoints.Remove(endpoint);
        await _db.SaveChangesAsync(ct);
        InvalidateCache();
        return true;
    }

    public async Task<ApiEndpointResponse?> ResolveAsync(string httpMethod, string path, CancellationToken ct = default)
    {
        var all = await GetCachedAsync(ct);
        var method = httpMethod.ToUpperInvariant();
        foreach (var endpoint in all.Where(e => e.HttpMethod == method && e.IsActive))
        {
            if (MatchTemplate(endpoint.Path, path)) return endpoint;
        }
        return null;
    }

    public void InvalidateCache() => _cache.Remove(CacheKey);

    private Task<IReadOnlyList<ApiEndpointResponse>> GetCachedAsync(CancellationToken ct)
    {
        return _cache.GetOrCreateAsync<IReadOnlyList<ApiEndpointResponse>>(CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheTtl;
            var rows = await _db.ApiEndpoints.AsNoTracking().ToListAsync(ct);
            return rows.Select(Project).ToList();
        })!;
    }

    private static bool MatchTemplate(string template, string actualPath)
    {
        try
        {
            var routeTemplate = TemplateParser.Parse(template.TrimStart('/'));
            var matcher = new TemplateMatcher(routeTemplate, new Microsoft.AspNetCore.Routing.RouteValueDictionary());
            return matcher.TryMatch(actualPath, new Microsoft.AspNetCore.Routing.RouteValueDictionary());
        }
        catch
        {
            return string.Equals(template, actualPath, StringComparison.OrdinalIgnoreCase);
        }
    }

    private static ApiEndpointResponse Project(ApiEndpoint e) => new()
    {
        Id = e.Id,
        Name = e.Name,
        Description = e.Description,
        Path = e.Path,
        HttpMethod = e.HttpMethod,
        IsActive = e.IsActive,
        RequiredPermissionCode = e.RequiredPermissionCode
    };
}
