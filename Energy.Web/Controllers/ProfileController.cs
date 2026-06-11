using System.Security.Claims;
using Energy.Localization;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Web.Clients.Identity;
using Energy.Web.Common;
using Energy.Web.Common.Filters;
using Energy.Web.Models.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Energy.Web.Controllers;

/// <summary>
/// Renders the current user's profile page (read-only summary of personal
/// info + roles + permissions) and proxies profile-image management calls
/// to the API. Linked from the top-right user menu and from the sidebar.
/// </summary>
[Authorize]
[Route("profile")]
[ServiceFilter(typeof(ApiExceptionFilter))]
public sealed class ProfileController : Controller
{
    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/png",
        "image/jpeg",
        "image/jpg",
        "image/gif",
        "image/webp"
    };

    private const long MaxImageSizeBytes = 2 * 1024 * 1024; // 2 MB

    private readonly IUserApiClient _userApiClient;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(
        IUserApiClient userApiClient,
        IStringLocalizer<SharedResource> localizer,
        ILogger<ProfileController> logger)
    {
        _userApiClient = userApiClient;
        _localizer = localizer;
        _logger = logger;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer.GetText(LocalizationKeys.ProfileScreen.Title);

        // Always render the page — even if the API call fails (e.g. the API
        // is restarting or the /me endpoint is not yet deployed) the user
        // should still see something useful built from the cookie claims
        // instead of being bounced back to the dashboard.
        UserDetailResponse detail;
        try
        {
            var envelope = await _userApiClient.GetCurrentUserAsync(cancellationToken);
            if (envelope.IsSuccess && envelope.Data is not null)
            {
                detail = envelope.Data;
            }
            else
            {
                _logger.LogWarning(
                    "GetCurrentUser returned an unsuccessful envelope: {Message}",
                    envelope.Message);
                detail = BuildDetailFromClaims();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "GetCurrentUser failed; falling back to claims-only profile data.");
            detail = BuildDetailFromClaims();
        }

        var permissions = User.Claims
            .Where(c => c.Type == EnergyClaimTypes.Permission)
            .Select(c => c.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(value => value, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var roleKeys = User.GetRoleKeys();

        return View(new ProfileViewModel
        {
            User = detail,
            Permissions = permissions,
            RoleKeys = roleKeys
        });
    }

    /// <summary>
    /// Builds a minimal <see cref="UserDetailResponse"/> from the claims
    /// already present on the cookie principal. Used as a fallback when the
    /// API call cannot satisfy the request.
    /// </summary>
    private UserDetailResponse BuildDetailFromClaims()
    {
        var userId = User.GetUserId() ?? Guid.Empty;
        var fullName = User.GetFullName() ?? string.Empty;
        var parts = fullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        var firstName = parts.Length > 0 ? parts[0] : string.Empty;
        var lastName = parts.Length > 1 ? parts[1] : string.Empty;
        var email = User.FindFirstValue(ClaimTypes.Email);
        var userName = User.FindFirstValue(ClaimTypes.Name);

        var roles = User.FindAll(ClaimTypes.Role)
            .Select(c => new RoleSummaryResponse { Id = Guid.Empty, Name = c.Value, Description = string.Empty })
            .ToList();

        return new UserDetailResponse
        {
            Id = userId,
            FirstName = firstName,
            LastName = lastName,
            IsActive = true,
            UserName = userName,
            Email = email,
            EmailConfirmed = false,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            HasProfileImage = false,
            Roles = roles
        };
    }

    /// <summary>
    /// Proxies the current user's avatar so the browser can render it with a
    /// stable URL (<c>/profile/image</c>) even though storage lives in the API.
    /// </summary>
    [HttpGet("image")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Image(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        if (userId is null)
        {
            return NotFound();
        }

        try
        {
            var image = await _userApiClient.GetProfileImageAsync(userId.Value, cancellationToken);
            if (image is null)
            {
                return NotFound();
            }

            return File(image.Value.Content, image.Value.ContentType);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Profile image proxy failed.");
            return NotFound();
        }
    }

    [HttpPost("image")]
    [IgnoreAntiforgeryToken]
    [RequestSizeLimit(MaxImageSizeBytes + 4096)]
    public async Task<IActionResult> UploadImage(IFormFile? file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest(new { message = _localizer.GetText(LocalizationKeys.Messages.ProfileImageEmpty) });
        }

        if (file.Length > MaxImageSizeBytes)
        {
            return BadRequest(new { message = _localizer.GetText(LocalizationKeys.Messages.ProfileImageTooLarge) });
        }

        var contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;
        if (!AllowedContentTypes.Contains(contentType))
        {
            return BadRequest(new { message = _localizer.GetText(LocalizationKeys.Messages.ProfileImageInvalidType) });
        }

        await using var stream = new MemoryStream();
        await file.CopyToAsync(stream, cancellationToken);

        // The API resolves the target user from the JWT subject — this avoids
        // any drift between the cookie principal's NameIdentifier and the row
        // currently present in the database (e.g. after a reseed).
        var envelope = await _userApiClient.UpdateMyProfileImageAsync(
            new UpdateProfileImageRequest
            {
                Content = stream.ToArray(),
                ContentType = contentType
            },
            cancellationToken);

        return envelope.ToJsonResult();
    }

    [HttpPost("image/remove")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> RemoveImage(CancellationToken cancellationToken)
    {
        var envelope = await _userApiClient.RemoveMyProfileImageAsync(cancellationToken);
        return envelope.ToJsonResult();
    }
}

