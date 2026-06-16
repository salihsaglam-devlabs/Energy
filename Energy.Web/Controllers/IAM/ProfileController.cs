using System.Security.Claims;
using Energy.Localization;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Web.Clients.Identity;
using Energy.Web.Common;
using Energy.Web.Common.Filters;
using Energy.Web.Models.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Energy.Web.Controllers.IAM;

[Authorize]
[PagePermission(PermissionCatalog.ProfileRead)]
[Route("profile")]
public sealed class ProfileController : Controller
{
    private readonly IUserApiClient _users;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(
        IUserApiClient users,
        IStringLocalizer<SharedResource> localizer,
        ILogger<ProfileController> logger)
    {
        _users = users;
        _localizer = localizer;
        _logger = logger;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        ViewData["Title"] = _localizer.GetText(LocalizationKeys.ProfileScreen.Title);

        var userId = User.GetUserId();
        var model = BuildFromClaims(userId);

        // API'nin kullanıcı bazlı kaydıyla en iyi çaba zenginleştirme. Hatalarda,
        // sayfanın her zaman görüntülenebilir olması için yalnızca claim'lere dayalı
        // görünüme geri düşülür.
        if (userId is Guid id)
        {
            try
            {
                var envelope = await _users.GetByIdAsync(id, ct);
                if (envelope.IsSuccess && envelope.Data is not null)
                {
                    var u = envelope.Data;
                    model = model with
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = string.IsNullOrEmpty(u.Email) ? model.Email : u.Email,
                        UserName = string.IsNullOrEmpty(u.UserName) ? model.UserName : u.UserName,
                        FullName = $"{u.FirstName} {u.LastName}".Trim(),
                        IsActive = u.IsActive,
                        Roles = u.Roles.Select(r => new ProfileRoleViewModel
                        {
                            Id = r.Id, Name = r.Name, Description = r.Description
                        }).ToArray(),
                        Permissions = u.EffectivePermissions.OrderBy(p => p, StringComparer.OrdinalIgnoreCase).ToArray()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "GetById failed for current user; using claims-only profile.");
            }

            try
            {
                var (_, _, status) = await _users.GetProfileImageAsync(id, ct);
                model = model with { HasProfileImage = status == StatusCodes.Status200OK };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Profile-image probe failed for current user.");
            }
        }

        return View((ProfileViewModel)model);
    }

    [HttpGet("image")]
    public async Task<IActionResult> Image(CancellationToken ct)
    {
        if (User.GetUserId() is not Guid id) return NotFound();
        var (content, contentType, status) = await _users.GetProfileImageAsync(id, ct);
        if (status != StatusCodes.Status200OK || content.Length == 0) return NotFound();
        return File(content, contentType);
    }

    [HttpPost("image")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> UploadImage(IFormFile? file, CancellationToken ct)
    {
        if (User.GetUserId() is not Guid id) return Unauthorized();
        if (file is null || file.Length == 0)
            return BadRequest(new { message = _localizer.GetText(LocalizationKeys.ProfileScreen.ImageHint) });

        await using var stream = new MemoryStream();
        await file.CopyToAsync(stream, ct);

        var envelope = await _users.SetProfileImageAsync(id, new SetProfileImageRequest
        {
            ContentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
            ContentBase64 = Convert.ToBase64String(stream.ToArray())
        }, ct);

        if (!envelope.IsSuccess)
            return BadRequest(new { message = envelope.Message });

        return Ok(new { success = true });
    }

    [HttpPost("image/remove")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> RemoveImage(CancellationToken ct)
    {
        if (User.GetUserId() is not Guid id) return Unauthorized();
        var envelope = await _users.RemoveProfileImageAsync(id, ct);
        return Json(envelope);
    }

    private ProfileViewModelRecord BuildFromClaims(Guid? userId)
    {
        var permissions = User.Claims
            .Where(c => c.Type == EnergyClaimTypes.Permission)
            .Select(c => c.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(v => v, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var roles = User.FindAll(ClaimTypes.Role)
            .Select(c => new ProfileRoleViewModel { Id = Guid.Empty, Name = c.Value })
            .ToArray();

        var fullName = User.GetFullName() ?? string.Empty;
        var parts = fullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        return new ProfileViewModelRecord
        {
            UserId = userId ?? Guid.Empty,
            UserName = User.Identity?.Name ?? string.Empty,
            Email = User.GetEmail() ?? string.Empty,
            FullName = fullName,
            FirstName = parts.Length > 0 ? parts[0] : string.Empty,
            LastName = parts.Length > 1 ? parts[1] : string.Empty,
            IsActive = true,
            Roles = roles,
            Permissions = permissions
        };
    }

    // Yerel "with" dostu kopyalama yardımcısı. Görünüm modeli yalnızca init özellikleriyle
    // oluşturulur; bu yüzden artımlı güncellemeler için onları bir record üzerinden yansıtırız.
    private sealed record ProfileViewModelRecord
    {
        public Guid UserId { get; init; }
        public string UserName { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        public bool IsActive { get; init; }
        public bool HasProfileImage { get; init; }
        public IReadOnlyList<ProfileRoleViewModel> Roles { get; init; } = Array.Empty<ProfileRoleViewModel>();
        public IReadOnlyList<string> Permissions { get; init; } = Array.Empty<string>();

        public static implicit operator ProfileViewModel(ProfileViewModelRecord r) => new()
        {
            UserId = r.UserId, UserName = r.UserName,
            FirstName = r.FirstName, LastName = r.LastName,
            Email = r.Email, FullName = r.FullName, IsActive = r.IsActive,
            HasProfileImage = r.HasProfileImage,
            Roles = r.Roles, Permissions = r.Permissions
        };
    }
}
