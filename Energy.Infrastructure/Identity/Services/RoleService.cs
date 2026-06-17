using Energy.Application.Common.Exceptions;
using Energy.Application.Identity.Services;
using Energy.Domain.IAM;
using Energy.Infrastructure.Persistence;
using Energy.Localization;
using Energy.Shared.Identity;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Energy.Infrastructure.Identity.Services;

public sealed class RoleService : IRoleService
{
    private readonly AppDbContext _db;
    private readonly IPermissionResolver _permissions;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public RoleService(AppDbContext db, IPermissionResolver permissions, IStringLocalizer<SharedResource> localizer)
    {
        _db = db;
        _permissions = permissions;
        _localizer = localizer;
    }

    public async Task<PaginatedResponse<RoleSummaryResponse>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default)
    {
        var query = _db.Roles.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(r => r.Name.ToLower().Contains(term));
        }
        var total = await query.CountAsync(ct);
        var rows = await query
            .OrderBy(r => r.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(r => new
            {
                r.Id,
                r.Name,
                r.Description,
                r.IsSystem,
                PermissionCount = _db.RolePermissions.Count(rp => rp.RoleId == r.Id),
                UserCount = _db.UserRoles.Count(ur => ur.RoleId == r.Id)
            })
            .ToListAsync(ct);

        var page = rows.Select(r => new RoleSummaryResponse
        {
            Id = r.Id,
            Name = r.Name,
            Description = LocalizeDescription(r.Description),
            IsSystem = r.IsSystem,
            PermissionCount = r.PermissionCount,
            UserCount = r.UserCount
        }).ToList();

        return PaginatedResponse<RoleSummaryResponse>.Create(page, request.PageNumber, request.PageSize, total);
    }

    public async Task<RoleDetailResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var role = await _db.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id, ct);
        if (role is null) return null;

        var codes = await _db.RolePermissions.AsNoTracking()
            .Where(rp => rp.RoleId == id)
            .Select(rp => rp.PermissionCode)
            .OrderBy(c => c)
            .ToListAsync(ct);

        var users = await _db.UserRoles.AsNoTracking()
            .Where(ur => ur.RoleId == id)
            .Join(_db.Users.AsNoTracking(), ur => ur.UserId, u => u.Id, (_, u) => u)
            .Select(u => new UserSummaryResponse
            {
                Id = u.Id, UserName = u.UserName, Email = u.Email,
                FullName = (u.FirstName + " " + u.LastName).Trim(),
                IsActive = u.IsActive, LastLoginAt = u.LastLoginAt
            })
            .ToListAsync(ct);

        return new RoleDetailResponse
        {
            Id = role.Id, Name = role.Name, Description = LocalizeDescription(role.Description), IsSystem = role.IsSystem,
            PermissionCodes = codes, Users = users
        };
    }

    public async Task<RoleDetailResponse> CreateAsync(CreateRoleRequest request, CancellationToken ct = default)
    {
        var name = request.Name.Trim();
        if (await _db.Roles.AnyAsync(r => r.Name == name, ct))
            throw new ConflictException(LocalizationKeys.Messages.RoleAlreadyExists, name);

        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = request.Description,
            IsSystem = false
        };
        _db.Roles.Add(role);
        await _db.SaveChangesAsync(ct);
        return (await GetByIdAsync(role.Id, ct))!;
    }

    public async Task<RoleDetailResponse> UpdateAsync(Guid id, UpdateRoleRequest request, CancellationToken ct = default)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == id, ct)
                   ?? throw new NotFoundException(LocalizationKeys.Messages.RoleNotFound, id);

        if (role.IsSystem && !string.Equals(role.Name, request.Name.Trim(), StringComparison.Ordinal))
            throw new ConflictException(LocalizationKeys.Messages.SystemRoleCannotBeRenamed);

        var name = request.Name.Trim();
        if (!string.Equals(role.Name, name, StringComparison.OrdinalIgnoreCase) &&
            await _db.Roles.AnyAsync(r => r.Name == name && r.Id != id, ct))
            throw new ConflictException(LocalizationKeys.Messages.RoleAlreadyExists, name);

        role.Name = name;
        role.Description = request.Description;
        await _db.SaveChangesAsync(ct);
        return (await GetByIdAsync(id, ct))!;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (role is null) return false;
        if (role.IsSystem) throw new ConflictException(LocalizationKeys.Messages.SystemRoleCannotBeDeleted);

        _db.Roles.Remove(role);
        await _db.SaveChangesAsync(ct);
        await _permissions.InvalidateRoleAsync(id, ct);
        return true;
    }

    public async Task<RoleDetailResponse> SetPermissionsAsync(Guid id, SetRolePermissionsRequest request, CancellationToken ct = default)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == id, ct)
                   ?? throw new NotFoundException(LocalizationKeys.Messages.RoleNotFound, id);

        if (string.Equals(role.Name, SystemRoles.SuperAdmin, StringComparison.OrdinalIgnoreCase))
            throw new ConflictException(LocalizationKeys.Messages.SuperAdminPermissionsAutoManaged);

        var desired = request.PermissionCodes
            .Where(code => PermissionCatalog.AllCodes.Contains(code))
            .Distinct()
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var existing = await _db.RolePermissions.Where(rp => rp.RoleId == id).ToListAsync(ct);
        var current = existing.Select(rp => rp.PermissionCode).ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var rp in existing.Where(rp => !desired.Contains(rp.PermissionCode))) _db.RolePermissions.Remove(rp);
        foreach (var code in desired.Where(c => !current.Contains(c)))
            _db.RolePermissions.Add(new RolePermission { RoleId = id, PermissionCode = code });

        await _db.SaveChangesAsync(ct);
        await _permissions.InvalidateRoleAsync(id, ct);

        return (await GetByIdAsync(id, ct))!;
    }

    /// <summary>
    /// Tohumlanan roller <c>Description</c> içinde bir yerelleştirme ANAHTARI saklar;
    /// kullanıcı tarafından oluşturulan roller serbest metin saklar. Anahtar varsa
    /// çözümle, aksi halde metni olduğu gibi döndür.
    /// </summary>
    private string? LocalizeDescription(string? value)
        => string.IsNullOrWhiteSpace(value) ? value : _localizer.GetText(value, value);
}
