using Energy.Localization;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Settings.Requests;
using Energy.Web.Clients.Settings;
using Energy.Web.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Energy.Web.Controllers;

/// <summary>
/// Self servis ayarlar ekranı. Kimliği doğrulanmış her kullanıcı yalnızca kendi
/// tercihlerini (bildirim sesi, tema, okundu bilgileri, ...) yönetir. Sayfa ve veri
/// uç noktaları varsayılan olarak verilen <c>UserSettings.Read</c> ile korunur; böylece
/// tüm roller ona ulaşabilir. Gerçek zamanlı istemci, bildirim seslerini çalıp
/// çalmayacağına karar vermek için <c>/settings/data</c>'yı da okur.
/// </summary>
[Authorize]
[PagePermission(PermissionCatalog.UserSettingsRead)]
[Route("settings")]
public sealed class SettingsController : Controller
{
    private readonly ISettingsApiClient _settings;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public SettingsController(ISettingsApiClient settings, IStringLocalizer<SharedResource> localizer)
    {
        _settings = settings;
        _localizer = localizer;
    }

    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index()
    {
        ViewData["Title"] = _localizer.GetText(LocalizationKeys.SettingsScreen.Title);
        return View();
    }

    [HttpGet("data")]
    public async Task<IActionResult> Data(CancellationToken ct)
    {
        var envelope = await _settings.GetMineAsync(ct);
        return Json(envelope.Data ?? new Shared.Models.V1.Settings.Responses.UserSettingsResponse());
    }

    public sealed class SaveInput
    {
        public bool NotificationSound { get; set; } = true;
        public bool CallSound { get; set; } = true;
        public bool DesktopNotifications { get; set; } = true;
        public bool ReadReceipts { get; set; } = true;
        public string Theme { get; set; } = "system";
    }

    [HttpPost("")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Save([FromBody] SaveInput input, CancellationToken ct)
    {
        var envelope = await _settings.UpdateMineAsync(new UpdateUserSettingsRequest
        {
            NotificationSound = input.NotificationSound,
            CallSound = input.CallSound,
            DesktopNotifications = input.DesktopNotifications,
            ReadReceipts = input.ReadReceipts,
            Theme = input.Theme
        }, ct);
        return Json(envelope);
    }
}

