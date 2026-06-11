using Energy.Application.Common.Exceptions;
using Energy.Application.Identity.Services;
using Energy.Domain.Identity;
using Energy.Infrastructure.Persistence;
using Energy.Localization;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Energy.Infrastructure.Identity.Services;

public sealed class PermissionService : IPermissionService
{
    private const string AdminRoleName = "Admin";

    // Default permission catalog used by SeedDefaultPermissionsAsync.
    // Aggregates all endpoint-specific permission codes via PermissionCatalog.
    private static readonly IReadOnlyList<PermissionDescriptor> DefaultPermissions = PermissionCatalog.All;

    private readonly AppDbContext _dbContext;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public PermissionService(AppDbContext dbContext, IStringLocalizer<SharedResource> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;
    }

    public async Task<IReadOnlyList<PermissionResponse>> GetPermissionsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Permissions
            .AsNoTracking()
            .OrderBy(p => p.Code)
            .Select(p => new PermissionResponse { Id = p.Id, Code = p.Code, Name = p.Name })
            .ToListAsync(cancellationToken);
    }

    public async Task<PermissionResponse> GetPermissionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var permission = await _dbContext.Permissions
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new PermissionResponse { Id = p.Id, Code = p.Code, Name = p.Name })
            .FirstOrDefaultAsync(cancellationToken);

        return permission ?? throw new NotFoundException(string.Format(
            _localizer.GetText(LocalizationKeys.Messages.PermissionNotFound, "Permission '{0}' was not found."),
            id));
    }

    public async Task<PermissionResponse> CreatePermissionAsync(CreatePermissionRequest request, CancellationToken cancellationToken = default)
    {
        var code = Normalize(request.Code);
        if (await _dbContext.Permissions.AnyAsync(p => p.Code == code, cancellationToken))
        {
            throw new ConflictException(string.Format(
                _localizer.GetText(LocalizationKeys.Messages.PermissionAlreadyExists, "Permission '{0}' already exists."),
                code));
        }

        var permission = new Permission
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = request.Name.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Permissions.Add(permission);

        // Keep Admin role as super-user: every newly created permission is linked automatically.
        var adminRole = await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.NormalizedName == AdminRoleName.ToUpperInvariant(), cancellationToken);

        if (adminRole is not null)
        {
            var adminHasPermission = await _dbContext.RolePermissions
                .AnyAsync(rp => rp.RoleId == adminRole.Id && rp.PermissionId == permission.Id, cancellationToken);

            if (!adminHasPermission)
            {
                _dbContext.RolePermissions.Add(new RolePermission
                {
                    RoleId = adminRole.Id,
                    PermissionId = permission.Id
                });
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new PermissionResponse { Id = permission.Id, Code = permission.Code, Name = permission.Name };
    }

    public async Task<PermissionResponse> UpdatePermissionAsync(Guid id, UpdatePermissionRequest request, CancellationToken cancellationToken = default)
    {
        var permission = await _dbContext.Permissions.FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
                         ?? throw new NotFoundException(string.Format(
                             _localizer.GetText(LocalizationKeys.Messages.PermissionNotFound, "Permission '{0}' was not found."),
                             id));

        var code = Normalize(request.Code);
        if (await _dbContext.Permissions.AnyAsync(p => p.Id != id && p.Code == code, cancellationToken))
        {
            throw new ConflictException(string.Format(
                _localizer.GetText(LocalizationKeys.Messages.PermissionAlreadyExists, "Permission '{0}' already exists."),
                code));
        }

        permission.Code = code;
        permission.Name = request.Name.Trim();
        permission.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new PermissionResponse { Id = permission.Id, Code = permission.Code, Name = permission.Name };
    }

    public async Task DeletePermissionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var permission = await _dbContext.Permissions.FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
                         ?? throw new NotFoundException(string.Format(
                             _localizer.GetText(LocalizationKeys.Messages.PermissionNotFound, "Permission '{0}' was not found."),
                             id));

        _dbContext.Permissions.Remove(permission);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<SeedResultResponse> SeedDefaultPermissionsAsync(CancellationToken cancellationToken = default)
    {
        var added = 0;
        var updated = 0;

        foreach (var (code, nameKey, fallbackName) in DefaultPermissions)
        {
            var localizedName = _localizer.GetText(nameKey, fallbackName);
            var existing = await _dbContext.Permissions.FirstOrDefaultAsync(p => p.Code == code, cancellationToken);
            if (existing is null)
            {
                _dbContext.Permissions.Add(new Permission
                {
                    Id = Guid.NewGuid(),
                    Code = code,
                    Name = localizedName,
                    CreatedAt = DateTime.UtcNow
                });
                added++;
            }
            else if (existing.Name != localizedName)
            {
                existing.Name = localizedName;
                existing.UpdatedAt = DateTime.UtcNow;
                updated++;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        var total = await _dbContext.Permissions.AsNoTracking().CountAsync(cancellationToken);
        return new SeedResultResponse { Added = added, Updated = updated, Total = total };
    }

    private static string Normalize(string value) => value.Trim().ToUpperInvariant();

    public async Task<SeedResultResponse> SeedPermissionCodesAsync(IEnumerable<string> codes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(codes);

        var distinct = codes
            .Where(static c => !string.IsNullOrWhiteSpace(c))
            .Select(static c => c.Trim())
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        if (distinct.Length == 0)
        {
            var totalEmpty = await _dbContext.Permissions.AsNoTracking().CountAsync(cancellationToken);
            return new SeedResultResponse { Added = 0, Updated = 0, Total = totalEmpty };
        }

        var existingCodes = await _dbContext.Permissions
            .Where(p => distinct.Contains(p.Code))
            .Select(p => p.Code)
            .ToListAsync(cancellationToken);

        var existingSet = new HashSet<string>(existingCodes, StringComparer.Ordinal);

        var added = 0;
        foreach (var code in distinct)
        {
            if (existingSet.Contains(code))
            {
                continue;
            }

            var nameKey = PermissionCatalog.BuildNameKey(code);
            var fallback = PermissionCatalog.BuildFallbackName(code);
            var localizedName = _localizer.GetText(nameKey, fallback);

            _dbContext.Permissions.Add(new Permission
            {
                Id = Guid.NewGuid(),
                Code = code,
                Name = localizedName,
                CreatedAt = DateTime.UtcNow
            });
            added++;
        }

        if (added > 0)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var total = await _dbContext.Permissions.AsNoTracking().CountAsync(cancellationToken);
        return new SeedResultResponse { Added = added, Updated = 0, Total = total };
    }
}

