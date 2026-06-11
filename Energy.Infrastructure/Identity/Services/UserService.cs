using Energy.Application.Common.Exceptions;
using Energy.Application.Identity.Services;
using Energy.Domain.Identity;
using Energy.Infrastructure.Identity.Services;
using Energy.Infrastructure.Persistence;
using Energy.Localization;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Energy.Infrastructure.Identity.Services;

public sealed class UserService : IUserService
{
    private readonly AppDbContext _dbContext;
    private readonly PasswordHashingService _passwordHashingService;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public UserService(
        AppDbContext dbContext,
        PasswordHashingService passwordHashingService,
        IStringLocalizer<SharedResource> localizer)
    {
        _dbContext = dbContext;
        _passwordHashingService = passwordHashingService;
        _localizer = localizer;
    }

    public async Task<IReadOnlyList<UserSummaryResponse>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _dbContext.Users
            .AsNoTracking()
            .OrderBy(user => user.UserName)
            .Select(user => new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.IsActive,
                user.UserName,
                user.Email,
                HasProfileImage = user.ProfileImage != null
            })
            .ToListAsync(cancellationToken);

        var roleLookup = await BuildRoleLookupAsync(users.Select(user => user.Id).ToArray(), cancellationToken);

        return users.Select(user => new UserSummaryResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            UserName = user.UserName,
            Email = user.Email,
            HasProfileImage = user.HasProfileImage,
            Roles = roleLookup.GetValueOrDefault(user.Id, [])
        }).ToList();
    }

    public async Task<UserDetailResponse> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new UserDetailResponse
            {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                IsActive = item.IsActive,
                UserName = item.UserName,
                NormalizedUserName = item.NormalizedUserName,
                Email = item.Email,
                NormalizedEmail = item.NormalizedEmail,
                EmailConfirmed = item.EmailConfirmed,
                PhoneNumber = item.PhoneNumber,
                PhoneNumberConfirmed = item.PhoneNumberConfirmed,
                TwoFactorEnabled = item.TwoFactorEnabled,
                LockoutEnd = item.LockoutEnd,
                LockoutEnabled = item.LockoutEnabled,
                AccessFailedCount = item.AccessFailedCount,
                HasProfileImage = item.ProfileImage != null,
                ProfileImageContentType = item.ProfileImageContentType
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(string.Format(
                _localizer.GetText(LocalizationKeys.Messages.UserNotFound, "User '{0}' was not found."),
                id));
        }

        var roleLookup = await BuildRoleLookupAsync([id], cancellationToken);
        return new UserDetailResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            UserName = user.UserName,
            NormalizedUserName = user.NormalizedUserName,
            Email = user.Email,
            NormalizedEmail = user.NormalizedEmail,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumber = user.PhoneNumber,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            TwoFactorEnabled = user.TwoFactorEnabled,
            LockoutEnd = user.LockoutEnd,
            LockoutEnabled = user.LockoutEnabled,
            AccessFailedCount = user.AccessFailedCount,
            HasProfileImage = user.HasProfileImage,
            ProfileImageContentType = user.ProfileImageContentType,
            Roles = roleLookup.GetValueOrDefault(id, [])
        };
    }

    public async Task<UserDetailResponse> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var normalizedUserName = Normalize(request.UserName);
        var userNameExists = await _dbContext.Users.AnyAsync(user => user.NormalizedUserName == normalizedUserName, cancellationToken);
        if (userNameExists)
        {
            throw new ConflictException(string.Format(
                _localizer.GetText(LocalizationKeys.Messages.UserNameAlreadyExists, "User name '{0}' already exists."),
                request.UserName));
        }

        await EnsureRolesExistAsync(request.RoleIds, cancellationToken);

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            IsActive = request.IsActive,
            UserName = request.UserName.Trim(),
            NormalizedUserName = normalizedUserName,
            Email = NormalizeOptional(request.Email, trimOnly: true),
            NormalizedEmail = NormalizeOptional(request.Email),
            EmailConfirmed = request.EmailConfirmed,
            PasswordHash = _passwordHashingService.HashPassword(request.Password),
            SecurityStamp = Guid.NewGuid().ToString("N"),
            ConcurrencyStamp = Guid.NewGuid().ToString("N"),
            PhoneNumber = NormalizeOptional(request.PhoneNumber, trimOnly: true),
            PhoneNumberConfirmed = request.PhoneNumberConfirmed,
            TwoFactorEnabled = request.TwoFactorEnabled,
            LockoutEnabled = request.LockoutEnabled,
            AccessFailedCount = 0
        };

        _dbContext.Users.Add(user);
        AddUserRoles(user.Id, request.RoleIds);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetUserByIdAsync(user.Id, cancellationToken);
    }

    public async Task<UserDetailResponse> UpdateUserAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
                   ?? throw new NotFoundException(string.Format(
                       _localizer.GetText(LocalizationKeys.Messages.UserNotFound, "User '{0}' was not found."),
                       id));

        var normalizedUserName = Normalize(request.UserName);
        var userNameExists = await _dbContext.Users.AnyAsync(
            item => item.Id != id && item.NormalizedUserName == normalizedUserName,
            cancellationToken);

        if (userNameExists)
        {
            throw new ConflictException(string.Format(
                _localizer.GetText(LocalizationKeys.Messages.UserNameAlreadyExists, "User name '{0}' already exists."),
                request.UserName));
        }

        user.FirstName = request.FirstName.Trim();
        user.LastName = request.LastName.Trim();
        user.IsActive = request.IsActive;
        user.UserName = request.UserName.Trim();
        user.NormalizedUserName = normalizedUserName;
        user.Email = NormalizeOptional(request.Email, trimOnly: true);
        user.NormalizedEmail = NormalizeOptional(request.Email);
        user.EmailConfirmed = request.EmailConfirmed;
        user.PhoneNumber = NormalizeOptional(request.PhoneNumber, trimOnly: true);
        user.PhoneNumberConfirmed = request.PhoneNumberConfirmed;
        user.TwoFactorEnabled = request.TwoFactorEnabled;
        user.LockoutEnabled = request.LockoutEnabled;
        user.LockoutEnd = request.LockoutEnd;
        user.ConcurrencyStamp = Guid.NewGuid().ToString("N");

        await _dbContext.SaveChangesAsync(cancellationToken);
        return await GetUserByIdAsync(id, cancellationToken);
    }

    public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
                   ?? throw new NotFoundException(string.Format(
                       _localizer.GetText(LocalizationKeys.Messages.UserNotFound, "User '{0}' was not found."),
                       id));

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserDetailResponse> SetUserRolesAsync(Guid id, IReadOnlyList<Guid> roleIds, CancellationToken cancellationToken = default)
    {
        var userExists = await _dbContext.Users.AnyAsync(item => item.Id == id, cancellationToken);
        if (!userExists)
        {
            throw new NotFoundException(string.Format(
                _localizer.GetText(LocalizationKeys.Messages.UserNotFound, "User '{0}' was not found."),
                id));
        }

        await EnsureRolesExistAsync(roleIds, cancellationToken);

        var existingRoles = await _dbContext.UserRoles.Where(item => item.UserId == id).ToListAsync(cancellationToken);
        _dbContext.UserRoles.RemoveRange(existingRoles);
        AddUserRoles(id, roleIds);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetUserByIdAsync(id, cancellationToken);
    }

    public async Task UpdatePasswordAsync(Guid id, string newPassword, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
                   ?? throw new NotFoundException(string.Format(
                       _localizer.GetText(LocalizationKeys.Messages.UserNotFound, "User '{0}' was not found."),
                       id));

        user.PasswordHash = _passwordHashingService.HashPassword(newPassword);
        user.SecurityStamp = Guid.NewGuid().ToString("N");
        user.ConcurrencyStamp = Guid.NewGuid().ToString("N");
        user.AccessFailedCount = 0;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<CredentialValidationResponse> ValidateCredentialsAsync(ValidateCredentialsRequest request, CancellationToken cancellationToken = default)
    {
        var normalized = Normalize(request.UserNameOrEmail);
        var user = await _dbContext.Users.FirstOrDefaultAsync(
            item => item.NormalizedUserName == normalized || item.NormalizedEmail == normalized,
            cancellationToken);

        if (user is null)
        {
            return new CredentialValidationResponse { IsAuthenticated = false };
        }

        var isLockedOut = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow;
        if (!user.IsActive || isLockedOut)
        {
            return await BuildCredentialResponseAsync(user, false, isLockedOut, cancellationToken);
        }

        var verified = _passwordHashingService.VerifyPassword(request.Password, user.PasswordHash);
        if (!verified)
        {
            user.AccessFailedCount += 1;
            user.ConcurrencyStamp = Guid.NewGuid().ToString("N");
            await _dbContext.SaveChangesAsync(cancellationToken);
            return await BuildCredentialResponseAsync(user, false, false, cancellationToken);
        }

        user.AccessFailedCount = 0;
        user.ConcurrencyStamp = Guid.NewGuid().ToString("N");
        await _dbContext.SaveChangesAsync(cancellationToken);
        return await BuildCredentialResponseAsync(user, true, false, cancellationToken);
    }

    private async Task<CredentialValidationResponse> BuildCredentialResponseAsync(User user, bool authenticated, bool isLockedOut, CancellationToken cancellationToken)
    {
        var rolePairs = await _dbContext.UserRoles
            .Where(userRole => userRole.UserId == user.Id)
            .Join(_dbContext.Roles, userRole => userRole.RoleId, role => role.Id, (_, role) => new
            {
                Name = role.Name ?? string.Empty,
                Key = role.NormalizedName ?? string.Empty
            })
            .OrderBy(item => item.Name)
            .ToListAsync(cancellationToken);

        var roles = rolePairs.Select(item => item.Name).ToList();
        var roleKeys = rolePairs
            .Select(item => item.Key)
            .Where(key => !string.IsNullOrWhiteSpace(key))
            .ToList();

        return new CredentialValidationResponse
        {
            IsAuthenticated = authenticated,
            UserId = authenticated ? user.Id : user.Id,
            UserName = user.UserName,
            Email = user.Email,
            IsActive = user.IsActive,
            IsLockedOut = isLockedOut,
            Roles = roles,
            RoleKeys = roleKeys
        };
    }

    private async Task EnsureRolesExistAsync(IReadOnlyList<Guid> roleIds, CancellationToken cancellationToken)
    {
        var distinctRoleIds = roleIds.Distinct().ToArray();
        if (distinctRoleIds.Length == 0)
        {
            return;
        }

        var matchedRoleCount = await _dbContext.Roles.CountAsync(role => distinctRoleIds.Contains(role.Id), cancellationToken);
        if (matchedRoleCount != distinctRoleIds.Length)
        {
            throw new NotFoundException(_localizer.GetText(
                LocalizationKeys.Messages.UserRolesNotFound,
                "One or more roles were not found."));
        }
    }

    private void AddUserRoles(Guid userId, IReadOnlyList<Guid> roleIds)
    {
        foreach (var roleId in roleIds.Distinct())
        {
            _dbContext.UserRoles.Add(new UserRole
            {
                UserId = userId,
                RoleId = roleId
            });
        }
    }

    private async Task<Dictionary<Guid, IReadOnlyList<RoleSummaryResponse>>> BuildRoleLookupAsync(IReadOnlyCollection<Guid> userIds, CancellationToken cancellationToken)
    {
        var pairs = await _dbContext.UserRoles
            .Where(userRole => userIds.Contains(userRole.UserId))
            .Join(
                _dbContext.Roles,
                userRole => userRole.RoleId,
                role => role.Id,
                (userRole, role) => new
                {
                    userRole.UserId,
                    Role = new RoleSummaryResponse
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Description = role.Description
                    }
                })
            .ToListAsync(cancellationToken);

        return pairs
            .GroupBy(pair => pair.UserId)
            .ToDictionary(group => group.Key, group => (IReadOnlyList<RoleSummaryResponse>)group.Select(item => item.Role).ToList());
    }

    private static string Normalize(string value) => value.Trim().ToUpperInvariant();

    private static string? NormalizeOptional(string? value, bool trimOnly = false)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return trimOnly ? value.Trim() : Normalize(value);
    }

    private const string AdminRoleName = "Admin";
    private const string AdminEmail = "admin@energy.local";
    private const string AdminDefaultPassword = "Admin123!";

    public async Task<SeedAdminResponse> SeedAdminAsync(CancellationToken cancellationToken = default)
    {
        var adminDescription = _localizer.GetText(LocalizationKeys.Roles.AdminDescription, AdminRoleName);
        var adminFirstName = _localizer.GetText(LocalizationKeys.Users.AdminFirstName, "System");
        var adminLastName = _localizer.GetText(LocalizationKeys.Users.AdminLastName, "Admin");

        var normalizedRoleName = AdminRoleName.ToUpperInvariant();
        var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);
        var roleCreated = false;
        if (role is null)
        {
            role = new Role
            {
                Id = Guid.NewGuid(),
                // Use the stable English name as the persisted Role.Name so that
                // identity checks like IsInRole("Admin") and the role_key claim
                // (NormalizedName) stay culture-independent. UI screens can still
                // rename it but the seeder will heal it back on next startup.
                Name = AdminRoleName,
                NormalizedName = normalizedRoleName,
                Description = adminDescription,
                ConcurrencyStamp = Guid.NewGuid().ToString("N")
            };
            _dbContext.Roles.Add(role);
            roleCreated = true;
        }
        else if (!string.Equals(role.Name, AdminRoleName, StringComparison.Ordinal))
        {
            // Heal databases that were seeded with a localized name (e.g.
            // "Yönetici" under tr-TR culture). A drifting display name breaks
            // user.IsInRole("Admin") in the Web/Api layers.
            role.Name = AdminRoleName;
            role.ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        var normalizedEmail = AdminEmail.ToUpperInvariant();
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
        var userCreated = false;
        if (user is null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                UserName = AdminEmail,
                NormalizedUserName = normalizedEmail,
                Email = AdminEmail,
                NormalizedEmail = normalizedEmail,
                FirstName = adminFirstName,
                LastName = adminLastName,
                EmailConfirmed = true,
                IsActive = true,
                PasswordHash = _passwordHashingService.HashPassword(AdminDefaultPassword),
                SecurityStamp = Guid.NewGuid().ToString("N"),
                ConcurrencyStamp = Guid.NewGuid().ToString("N")
            };
            _dbContext.Users.Add(user);
            userCreated = true;
        }

        var hasRoleLink = await _dbContext.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id, cancellationToken);
        if (!hasRoleLink)
        {
            _dbContext.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
        }

        // Ensure all default permissions exist and are linked to the Admin role.
        await EnsureDefaultPermissionsLinkedToRoleAsync(role.Id, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new SeedAdminResponse
        {
            UserId = user.Id,
            RoleId = role.Id,
            Email = AdminEmail,
            DefaultPassword = userCreated ? AdminDefaultPassword : string.Empty,
            UserCreated = userCreated,
            RoleCreated = roleCreated
        };
    }

    public async Task<IReadOnlyList<string>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserRoles
            .Where(userRole => userRole.UserId == userId)
            .Join(_dbContext.RolePermissions, ur => ur.RoleId, rp => rp.RoleId, (_, rp) => rp.PermissionId)
            .Join(_dbContext.Permissions, permissionId => permissionId, p => p.Id, (_, p) => p.Code)
            .Distinct()
            .OrderBy(code => code)
            .ToListAsync(cancellationToken);
    }

    public async Task<AdminPermissionHealthResponse> GetAdminPermissionHealthAsync(CancellationToken cancellationToken = default)
    {
        var allPermissionCodes = await _dbContext.Permissions
            .AsNoTracking()
            .Select(p => p.Code)
            .OrderBy(code => code)
            .ToListAsync(cancellationToken);

        var adminRole = await _dbContext.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.NormalizedName == AdminRoleName.ToUpperInvariant(), cancellationToken);

        if (adminRole is null)
        {
            return new AdminPermissionHealthResponse
            {
                AdminRoleExists = false,
                TotalPermissions = allPermissionCodes.Count,
                AssignedPermissions = 0,
                MissingPermissionCodes = allPermissionCodes
            };
        }

        var assignedCodes = await _dbContext.RolePermissions
            .Where(rp => rp.RoleId == adminRole.Id)
            .Join(_dbContext.Permissions, rp => rp.PermissionId, p => p.Id, (_, p) => p.Code)
            .Distinct()
            .OrderBy(code => code)
            .ToListAsync(cancellationToken);

        var missingCodes = allPermissionCodes
            .Except(assignedCodes, StringComparer.OrdinalIgnoreCase)
            .OrderBy(code => code)
            .ToList();

        return new AdminPermissionHealthResponse
        {
            AdminRoleExists = true,
            TotalPermissions = allPermissionCodes.Count,
            AssignedPermissions = assignedCodes.Count,
            MissingPermissionCodes = missingCodes
        };
    }

    private async Task EnsureDefaultPermissionsLinkedToRoleAsync(Guid roleId, CancellationToken cancellationToken)    {
        // Step 1: ensure every default permission row exists in the catalog.
        foreach (var (code, _, fallbackName) in PermissionCatalog.All)
        {
            var exists = await _dbContext.Permissions.AnyAsync(p => p.Code == code, cancellationToken);
            if (!exists)
            {
                _dbContext.Permissions.Add(new Permission
                {
                    Id = Guid.NewGuid(),
                    Code = code,
                    Name = fallbackName,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        // Persist new permissions before linking so we have stable Ids to query.
        await _dbContext.SaveChangesAsync(cancellationToken);

        var allPermissionIds = await _dbContext.Permissions
            .Select(p => p.Id)
            .ToListAsync(cancellationToken);

        var existingLinks = await _dbContext.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.PermissionId)
            .ToListAsync(cancellationToken);

        var missingPermissionIds = allPermissionIds.Except(existingLinks).ToList();
        foreach (var permissionId in missingPermissionIds)
        {
            _dbContext.RolePermissions.Add(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            });
        }
    }

    public async Task<ProfileImageResponse?> GetProfileImageAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var image = await _dbContext.Users
            .AsNoTracking()
            .Where(item => item.Id == userId)
            .Select(item => new { item.ProfileImage, item.ProfileImageContentType })
            .FirstOrDefaultAsync(cancellationToken);

        if (image is null || image.ProfileImage is null || image.ProfileImage.Length == 0)
        {
            return null;
        }

        return new ProfileImageResponse
        {
            Content = image.ProfileImage,
            ContentType = string.IsNullOrWhiteSpace(image.ProfileImageContentType)
                ? "application/octet-stream"
                : image.ProfileImageContentType!
        };
    }

    public async Task<UserDetailResponse> SetProfileImageAsync(Guid userId, byte[] content, string contentType, CancellationToken cancellationToken = default)
    {
        if (content is null || content.Length == 0)
        {
            throw new ConflictException(_localizer.GetText(
                LocalizationKeys.Messages.ProfileImageEmpty,
                "Profile image content cannot be empty."));
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(item => item.Id == userId, cancellationToken)
                   ?? throw new NotFoundException(string.Format(
                       _localizer.GetText(LocalizationKeys.Messages.UserNotFound, "User '{0}' was not found."),
                       userId));

        user.ProfileImage = content;
        user.ProfileImageContentType = string.IsNullOrWhiteSpace(contentType)
            ? "application/octet-stream"
            : contentType.Trim();
        user.ConcurrencyStamp = Guid.NewGuid().ToString("N");

        await _dbContext.SaveChangesAsync(cancellationToken);
        return await GetUserByIdAsync(userId, cancellationToken);
    }

    public async Task<UserDetailResponse> RemoveProfileImageAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(item => item.Id == userId, cancellationToken)
                   ?? throw new NotFoundException(string.Format(
                       _localizer.GetText(LocalizationKeys.Messages.UserNotFound, "User '{0}' was not found."),
                       userId));

        user.ProfileImage = null;
        user.ProfileImageContentType = null;
        user.ConcurrencyStamp = Guid.NewGuid().ToString("N");

        await _dbContext.SaveChangesAsync(cancellationToken);
        return await GetUserByIdAsync(userId, cancellationToken);
    }

    public async Task<Guid?> ResolveCurrentUserIdAsync(
        Guid? claimUserId,
        string? email,
        string? userName,
        CancellationToken cancellationToken = default)
    {
        if (claimUserId.HasValue && claimUserId.Value != Guid.Empty)
        {
            var byId = await _dbContext.Users
                .AsNoTracking()
                .Where(user => user.Id == claimUserId.Value)
                .Select(user => (Guid?)user.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (byId.HasValue)
            {
                return byId;
            }
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            var normalizedEmail = email.Trim().ToUpperInvariant();
            var byEmail = await _dbContext.Users
                .AsNoTracking()
                .Where(user => user.NormalizedEmail == normalizedEmail)
                .Select(user => (Guid?)user.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (byEmail.HasValue)
            {
                return byEmail;
            }
        }

        if (!string.IsNullOrWhiteSpace(userName))
        {
            var normalizedUserName = userName.Trim().ToUpperInvariant();
            var byUserName = await _dbContext.Users
                .AsNoTracking()
                .Where(user => user.NormalizedUserName == normalizedUserName)
                .Select(user => (Guid?)user.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (byUserName.HasValue)
            {
                return byUserName;
            }
        }

        return null;
    }
}
