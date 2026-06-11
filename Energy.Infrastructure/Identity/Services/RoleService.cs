using Energy.Application.Common.Exceptions;
using Energy.Application.Identity.Services;
using Energy.Domain.Identity;
using Energy.Domain.System;
using Energy.Infrastructure.Persistence;
using Energy.Localization;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Shared.Models.V1.System.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Energy.Infrastructure.Identity.Services;

public sealed class RoleService : IRoleService
{
    private readonly AppDbContext _dbContext;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public RoleService(AppDbContext dbContext, IStringLocalizer<SharedResource> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;
    }

    public async Task<IReadOnlyList<RoleSummaryResponse>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Roles
            .AsNoTracking()
            .OrderBy(role => role.Name)
            .Select(role => new RoleSummaryResponse
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<RoleDetailResponse> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = await _dbContext.Roles
            .AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new RoleDetailResponse
            {
                Id = item.Id,
                Name = item.Name,
                NormalizedName = item.NormalizedName,
                Description = item.Description,
                AssignedUserCount = _dbContext.UserRoles.Count(userRole => userRole.RoleId == item.Id)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return role ?? throw new NotFoundException(string.Format(
            _localizer.GetText(LocalizationKeys.Messages.RoleNotFound, "Role '{0}' was not found."),
            id));
    }

    public async Task<RoleDetailResponse> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default)
    {
        var normalizedName = Normalize(request.Name);
        var exists = await _dbContext.Roles.AnyAsync(role => role.NormalizedName == normalizedName, cancellationToken);
        if (exists)
        {
            throw new ConflictException(string.Format(
                _localizer.GetText(LocalizationKeys.Messages.RoleAlreadyExists, "Role '{0}' already exists."),
                request.Name));
        }

        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            NormalizedName = normalizedName,
            Description = request.Description.Trim(),
            ConcurrencyStamp = Guid.NewGuid().ToString("N")
        };

        _dbContext.Roles.Add(role);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetRoleByIdAsync(role.Id, cancellationToken);
    }

    public async Task<RoleDetailResponse> UpdateRoleAsync(Guid id, UpdateRoleRequest request, CancellationToken cancellationToken = default)
    {
        var role = await _dbContext.Roles.FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
                   ?? throw new NotFoundException(string.Format(
                       _localizer.GetText(LocalizationKeys.Messages.RoleNotFound, "Role '{0}' was not found."),
                       id));

        var normalizedName = Normalize(request.Name);
        var exists = await _dbContext.Roles.AnyAsync(
            item => item.Id != id && item.NormalizedName == normalizedName,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(string.Format(
                _localizer.GetText(LocalizationKeys.Messages.RoleAlreadyExists, "Role '{0}' already exists."),
                request.Name));
        }

        role.Name = request.Name.Trim();
        role.NormalizedName = normalizedName;
        role.Description = request.Description.Trim();
        role.ConcurrencyStamp = Guid.NewGuid().ToString("N");

        await _dbContext.SaveChangesAsync(cancellationToken);
        return await GetRoleByIdAsync(role.Id, cancellationToken);
    }

    public async Task DeleteRoleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = await _dbContext.Roles.FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
                   ?? throw new NotFoundException(string.Format(
                       _localizer.GetText(LocalizationKeys.Messages.RoleNotFound, "Role '{0}' was not found."),
                       id));

        _dbContext.Roles.Remove(role);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static string Normalize(string value) => value.Trim().ToUpperInvariant();

    public async Task<IReadOnlyList<PermissionResponse>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        await EnsureRoleExistsAsync(roleId, cancellationToken);

        return await _dbContext.RolePermissions
            .AsNoTracking()
            .Where(rp => rp.RoleId == roleId)
            .Join(_dbContext.Permissions.AsNoTracking(),
                rp => rp.PermissionId,
                p => p.Id,
                (rp, p) => new PermissionResponse { Id = p.Id, Code = p.Code, Name = p.Name })
            .OrderBy(p => p.Code)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PermissionResponse>> SetRolePermissionsAsync(Guid roleId, IReadOnlyCollection<Guid> permissionIds, CancellationToken cancellationToken = default)
    {
        await EnsureRoleExistsAsync(roleId, cancellationToken);

        var distinctIds = permissionIds?.Distinct().ToArray() ?? Array.Empty<Guid>();
        if (distinctIds.Length > 0)
        {
            var existingCount = await _dbContext.Permissions.AsNoTracking().CountAsync(p => distinctIds.Contains(p.Id), cancellationToken);
            if (existingCount != distinctIds.Length)
            {
                throw new NotFoundException(_localizer.GetText(
                    LocalizationKeys.Messages.PermissionsNotFound,
                    "One or more permissions were not found."));
            }
        }

        var existingLinks = await _dbContext.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .ToListAsync(cancellationToken);

        var toRemove = existingLinks.Where(rp => !distinctIds.Contains(rp.PermissionId)).ToList();
        var existingIds = existingLinks.Select(rp => rp.PermissionId).ToHashSet();
        var toAdd = distinctIds.Where(id => !existingIds.Contains(id))
            .Select(id => new RolePermission { RoleId = roleId, PermissionId = id });

        if (toRemove.Count > 0) _dbContext.RolePermissions.RemoveRange(toRemove);
        await _dbContext.RolePermissions.AddRangeAsync(toAdd, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetRolePermissionsAsync(roleId, cancellationToken);
    }

    public async Task<IReadOnlyList<MenuResponse>> GetRoleMenusAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        await EnsureRoleExistsAsync(roleId, cancellationToken);

        return await _dbContext.RoleMenus
            .AsNoTracking()
            .Where(rm => rm.RoleId == roleId)
            .Join(_dbContext.Menus.AsNoTracking(),
                rm => rm.MenuId,
                m => m.Id,
                (rm, m) => new MenuResponse
                {
                    Id = m.Id,
                    Name = m.Name,
                    Url = m.Url,
                    Icon = m.Icon,
                    Order = m.Order,
                    ParentId = m.ParentId
                })
            .OrderBy(m => m.Order)
            .ThenBy(m => m.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<MenuResponse>> SetRoleMenusAsync(Guid roleId, IReadOnlyCollection<Guid> menuIds, CancellationToken cancellationToken = default)
    {
        await EnsureRoleExistsAsync(roleId, cancellationToken);

        var distinctIds = menuIds?.Distinct().ToArray() ?? Array.Empty<Guid>();
        if (distinctIds.Length > 0)
        {
            var existingCount = await _dbContext.Menus.AsNoTracking().CountAsync(m => distinctIds.Contains(m.Id), cancellationToken);
            if (existingCount != distinctIds.Length)
            {
                throw new NotFoundException(_localizer.GetText(
                    LocalizationKeys.Messages.MenusNotFound,
                    "One or more menus were not found."));
            }
        }

        var existingLinks = await _dbContext.RoleMenus
            .Where(rm => rm.RoleId == roleId)
            .ToListAsync(cancellationToken);

        var toRemove = existingLinks.Where(rm => !distinctIds.Contains(rm.MenuId)).ToList();
        var existingIds = existingLinks.Select(rm => rm.MenuId).ToHashSet();
        var toAdd = distinctIds.Where(id => !existingIds.Contains(id))
            .Select(id => new RoleMenu { RoleId = roleId, MenuId = id });

        if (toRemove.Count > 0) _dbContext.RoleMenus.RemoveRange(toRemove);
        await _dbContext.RoleMenus.AddRangeAsync(toAdd, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetRoleMenusAsync(roleId, cancellationToken);
    }

    private async Task EnsureRoleExistsAsync(Guid roleId, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.Roles.AsNoTracking().AnyAsync(r => r.Id == roleId, cancellationToken);
        if (!exists)
        {
            throw new NotFoundException(string.Format(
                _localizer.GetText(LocalizationKeys.Messages.RoleNotFound, "Role '{0}' was not found."),
                roleId));
        }
    }
}
