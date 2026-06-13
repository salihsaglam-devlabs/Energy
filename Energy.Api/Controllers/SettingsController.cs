using Asp.Versioning;
using Energy.Application.Identity.Services;
using Energy.Application.Settings.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Settings.Requests;
using Energy.Shared.Models.V1.Settings.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

/// <summary>
/// Self servis, kullanıcı bazlı ayarlar. Her kimliği doğrulanmış kullanıcı yalnızca
/// kendi satırını okur/günceller (kimlik jetondan alınır, asla istekten alınmaz);
/// bu yüzden uç noktalar tüm rollere varsayılan olarak verilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/settings")]
public sealed class SettingsController : ControllerBase
{
    private readonly IUserSettingsService _settings;
    private readonly ICurrentUser _currentUser;

    public SettingsController(IUserSettingsService settings, ICurrentUser currentUser)
    {
        _settings = settings;
        _currentUser = currentUser;
    }

    private Guid CurrentUserId => _currentUser.UserId ?? Guid.Empty;

    [HttpGet("me")]
    public async Task<ActionResult<BaseResponse<UserSettingsResponse>>> GetMine(CancellationToken ct)
        => Ok(BaseResponse<UserSettingsResponse>.Success(await _settings.GetAsync(CurrentUserId, ct)));

    [HttpPut("me")]
    public async Task<ActionResult<BaseResponse<UserSettingsResponse>>> UpdateMine(UpdateUserSettingsRequest request, CancellationToken ct)
        => Ok(BaseResponse<UserSettingsResponse>.Success(await _settings.UpdateAsync(CurrentUserId, request, ct)));
}

