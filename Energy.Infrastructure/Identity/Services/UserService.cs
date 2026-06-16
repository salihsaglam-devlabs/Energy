using Energy.Application.Common.Exceptions;
using Energy.Application.Identity.Services;
using Energy.Domain.Modules.IAM;
using Energy.Infrastructure.Persistence;
using Energy.Localization;
using Energy.Shared.Identity;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Identity.Services;

public sealed class UserService : IUserService
{
    private readonly AppDbContext _db;
    private readonly PasswordHashingService _passwords;
    private readonly IJwtTokenService _tokens;
    private readonly IPermissionResolver _permissions;
    private readonly ILogger<UserService> _logger;

    public UserService(
        AppDbContext db,
        PasswordHashingService passwords,
        IJwtTokenService tokens,
        IPermissionResolver permissions,
        ILogger<UserService> logger)
    {
        _db = db;
        _passwords = passwords;
        _tokens = tokens;
        _permissions = permissions;
        _logger = logger;
    }

    public async Task<PaginatedResponse<UserSummaryResponse>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default)
    {
        var query = _db.Users.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(u => u.UserName.ToLower().Contains(term)
                                  || u.Email.ToLower().Contains(term)
                                  || u.FirstName.ToLower().Contains(term)
                                  || u.LastName.ToLower().Contains(term));
        }

        var total = await query.CountAsync(ct);
        var page = await query
            .OrderBy(u => u.UserName)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new
            {
                u.Id, u.UserName, u.Email, u.FirstName, u.LastName, u.IsActive, u.LastLoginAt,
                Roles = _db.UserRoles.Where(ur => ur.UserId == u.Id)
                          .Join(_db.Roles, ur => ur.RoleId, r => r.Id, (_, r) => r.Name)
                          .ToList()
            })
            .ToListAsync(ct);

        var items = page.Select(u => new UserSummaryResponse
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            FullName = $"{u.FirstName} {u.LastName}".Trim(),
            IsActive = u.IsActive,
            LastLoginAt = u.LastLoginAt,
            RoleNames = u.Roles
        }).ToList();

        return PaginatedResponse<UserSummaryResponse>.Create(items, request.PageNumber, request.PageSize, total);
    }

    public async Task<UserDetailResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user is null) return null;

        var roles = await _db.UserRoles.AsNoTracking()
            .Where(ur => ur.UserId == id)
            .Join(_db.Roles.AsNoTracking(), ur => ur.RoleId, r => r.Id, (_, r) => r)
            .Select(r => new RoleSummaryResponse
            {
                Id = r.Id, Name = r.Name, Description = r.Description, IsSystem = r.IsSystem
            })
            .ToListAsync(ct);

        var perms = await _permissions.GetPermissionsAsync(id, ct);

        return new UserDetailResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt,
            Roles = roles,
            EffectivePermissions = perms.OrderBy(c => c).ToList()
        };
    }

    public async Task<UserDetailResponse> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        var userName = request.UserName.Trim();
        var email = request.Email.Trim();

        if (await _db.Users.AnyAsync(u => u.UserName == userName, ct))
            throw new ConflictException(LocalizationKeys.Messages.UserNameAlreadyExists, userName);
        if (await _db.Users.AnyAsync(u => u.Email == email, ct))
            throw new ConflictException(LocalizationKeys.Messages.UserEmailAlreadyExists, email);

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            Email = email,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            PasswordHash = _passwords.Hash(request.Password),
            IsActive = request.IsActive,
            SecurityStamp = Guid.NewGuid()
        };
        _db.Users.Add(user);

        foreach (var roleId in request.RoleIds.Distinct())
        {
            _db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = roleId });
        }

        await _db.SaveChangesAsync(ct);
        return (await GetByIdAsync(user.Id, ct))!;
    }

    public async Task<UserDetailResponse> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken ct = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct)
                   ?? throw new NotFoundException(LocalizationKeys.Messages.UserNotFound, id);

        var email = request.Email.Trim();
        if (!string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase) &&
            await _db.Users.AnyAsync(u => u.Email == email && u.Id != id, ct))
        {
            throw new ConflictException(LocalizationKeys.Messages.UserEmailAlreadyExists, email);
        }

        var rolesChanged = false;
        var existingRoles = await _db.UserRoles.Where(ur => ur.UserId == id).ToListAsync(ct);
        var desired = request.RoleIds.Distinct().ToHashSet();
        var current = existingRoles.Select(r => r.RoleId).ToHashSet();
        foreach (var ur in existingRoles.Where(r => !desired.Contains(r.RoleId))) { _db.UserRoles.Remove(ur); rolesChanged = true; }
        foreach (var rid in desired.Where(r => !current.Contains(r))) { _db.UserRoles.Add(new UserRole { UserId = id, RoleId = rid }); rolesChanged = true; }

        var activationChanged = user.IsActive != request.IsActive;

        user.FirstName = request.FirstName.Trim();
        user.LastName = request.LastName.Trim();
        user.Email = email;
        user.IsActive = request.IsActive;
        if (rolesChanged || activationChanged) user.SecurityStamp = Guid.NewGuid();

        await _db.SaveChangesAsync(ct);
        _permissions.InvalidateUser(id);

        return (await GetByIdAsync(id, ct))!;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user is null) return false;

        _db.Users.Remove(user); // interceptor turns into soft delete
        await _db.SaveChangesAsync(ct);
        _permissions.InvalidateUser(id);
        return true;
    }

    public async Task<bool> ChangePasswordAsync(Guid id, ChangePasswordRequest request, CancellationToken ct = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user is null) return false;
        user.PasswordHash = _passwords.Hash(request.NewPassword);
        user.SecurityStamp = Guid.NewGuid();
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<UserAccessResponse?> GetAccessAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user is null) return null;

        var roleIds = await _db.UserRoles.AsNoTracking()
            .Where(ur => ur.UserId == id)
            .Select(ur => ur.RoleId)
            .ToListAsync(ct);

        var isSuperAdmin = await _db.UserRoles.AsNoTracking()
            .Where(ur => ur.UserId == id)
            .Join(_db.Roles.AsNoTracking(), ur => ur.RoleId, r => r.Id, (_, r) => r.Name)
            .AnyAsync(name => name == SystemRoles.SuperAdmin, ct);

        // Roller aracılığıyla devralınan yetkiler ekranda salt okunurdur.
        IReadOnlyList<string> rolePermissionCodes = isSuperAdmin
            ? PermissionCatalog.AllCodes.ToList()
            : await _db.UserRoles.AsNoTracking()
                .Where(ur => ur.UserId == id)
                .Join(_db.RolePermissions.AsNoTracking(), ur => ur.RoleId, rp => rp.RoleId, (_, rp) => rp.PermissionCode)
                .Distinct()
                .ToListAsync(ct);

        var directCodes = await _db.UserPermissions.AsNoTracking()
            .Where(up => up.UserId == id)
            .Select(up => up.PermissionCode)
            .ToListAsync(ct);

        return new UserAccessResponse
        {
            UserId = user.Id,
            UserName = user.UserName,
            FullName = $"{user.FirstName} {user.LastName}".Trim(),
            IsActive = user.IsActive,
            RoleIds = roleIds,
            RolePermissionCodes = rolePermissionCodes,
            DirectPermissionCodes = directCodes
        };
    }

    public async Task<UserAccessResponse> SetAccessAsync(Guid id, SetUserAccessRequest request, CancellationToken ct = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct)
                   ?? throw new NotFoundException(LocalizationKeys.Messages.UserNotFound, id);

        var changed = false;

        // --- Roller: istenen kümeye göre ekleme/kaldırma farkı ---
        var desiredRoles = request.RoleIds.Distinct().ToHashSet();
        var existingRoles = await _db.UserRoles.Where(ur => ur.UserId == id).ToListAsync(ct);
        var currentRoles = existingRoles.Select(ur => ur.RoleId).ToHashSet();
        foreach (var ur in existingRoles.Where(r => !desiredRoles.Contains(r.RoleId)))
        {
            _db.UserRoles.Remove(ur);
            changed = true;
        }
        foreach (var rid in desiredRoles.Where(r => !currentRoles.Contains(r)))
        {
            _db.UserRoles.Add(new UserRole { UserId = id, RoleId = rid });
            changed = true;
        }

        // --- Doğrudan yetkiler: yalnızca geçerli katalog kodlarını tut; mevcutla karşılaştır ---
        var desiredDirect = request.DirectPermissionCodes
            .Where(code => PermissionCatalog.AllCodes.Contains(code))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var existingDirect = await _db.UserPermissions.Where(up => up.UserId == id).ToListAsync(ct);
        var currentDirect = existingDirect.Select(up => up.PermissionCode).ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var up in existingDirect.Where(p => !desiredDirect.Contains(p.PermissionCode)))
        {
            _db.UserPermissions.Remove(up);
            changed = true;
        }
        foreach (var code in desiredDirect.Where(c => !currentDirect.Contains(c)))
        {
            _db.UserPermissions.Add(new UserPermission { UserId = id, PermissionCode = code });
            changed = true;
        }

        if (changed) user.SecurityStamp = Guid.NewGuid();
        await _db.SaveChangesAsync(ct);
        _permissions.InvalidateUser(id);

        return (await GetAccessAsync(id, ct))!;
    }

    public async Task<AuthTokenResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var identifier = request.UserNameOrEmail.Trim();
        var user = await _db.Users.FirstOrDefaultAsync(
            u => u.UserName == identifier || u.Email == identifier, ct);

        if (user is null)
        {
            // Yalnızca tanılama amaçlı — parolayı asla günlüğe yazmaz. Belirli bir
            // ortamda oturum açmanın neden başarısız olduğunu (ör. kullanıcının o
            // veritabanında bulunmaması) tespit etmeye yardımcı olur.
            _logger.LogWarning("[Login] FAILED: no user matches identifier '{Identifier}'.", identifier);
            return null;
        }
        if (!user.IsActive)
        {
            _logger.LogWarning("[Login] FAILED: user '{Identifier}' (Id={UserId}) is inactive.", identifier, user.Id);
            return null;
        }
        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
        {
            _logger.LogWarning("[Login] FAILED: user '{Identifier}' (Id={UserId}) is locked out until {LockoutEnd:o} (UTC now {Now:o}).",
                identifier, user.Id, user.LockoutEnd, DateTime.UtcNow);
            return null;
        }
        if (!_passwords.Verify(request.Password, user.PasswordHash))
        {
            user.FailedLoginCount += 1;
            if (user.FailedLoginCount >= 5) user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
            await _db.SaveChangesAsync(ct);
            _logger.LogWarning(
                "[Login] FAILED: wrong password for '{Identifier}' (Id={UserId}). FailedCount={FailedCount}{Locked}.",
                identifier, user.Id, user.FailedLoginCount,
                user.LockoutEnd is null ? string.Empty : $", locked until {user.LockoutEnd:o}");
            return null;
        }

        user.FailedLoginCount = 0;
        user.LockoutEnd = null;
        user.LastLoginAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        _logger.LogInformation("[Login] OK for '{Identifier}' (Id={UserId}).", identifier, user.Id);

        var token = _tokens.Issue(user);

        // Etkin yetki kümesini ve rol adlarını döndür; böylece Web katmanı menü /
        // sayfa / eylem yetkilendirmesini ek bir çağrı yapmadan yürütebilir.
        var roleNames = await _db.UserRoles.AsNoTracking()
            .Where(ur => ur.UserId == user.Id)
            .Join(_db.Roles.AsNoTracking(), ur => ur.RoleId, r => r.Id, (_, r) => r.Name)
            .ToListAsync(ct);

        var permissions = await _permissions.GetPermissionsAsync(user.Id, ct);

        return new AuthTokenResponse
        {
            AccessToken = token.AccessToken,
            ExpiresAt = token.ExpiresAt,
            UserId = token.UserId,
            UserName = token.UserName,
            DisplayName = token.DisplayName,
            Roles = roleNames,
            Permissions = permissions.ToArray()
        };
    }

    public async Task<ProfileImageResponse?> GetProfileImageAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _db.Users.AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new { u.ProfileImage, u.ProfileImageContentType })
            .FirstOrDefaultAsync(ct);
        if (user?.ProfileImage is null) return null;
        return new ProfileImageResponse
        {
            Content = user.ProfileImage,
            ContentType = user.ProfileImageContentType ?? "application/octet-stream"
        };
    }

    public async Task<bool> SetProfileImageAsync(Guid id, byte[] content, string contentType, CancellationToken ct = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user is null) return false;
        user.ProfileImage = content;
        user.ProfileImageContentType = contentType;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> RemoveProfileImageAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user is null) return false;
        user.ProfileImage = null;
        user.ProfileImageContentType = null;
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
