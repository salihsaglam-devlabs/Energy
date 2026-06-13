using Energy.Application.Settings.Services;
using Energy.Domain.Identity;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Settings.Requests;
using Energy.Shared.Models.V1.Settings.Responses;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Settings.Services;

public sealed class UserSettingsService : IUserSettingsService
{
    private static readonly string[] AllowedThemes = ["system", "light", "dark"];

    private readonly AppDbContext _db;

    public UserSettingsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<UserSettingsResponse> GetAsync(Guid userId, CancellationToken ct = default)
    {
        var row = await _db.UserSettings.AsNoTracking().FirstOrDefaultAsync(s => s.UserId == userId, ct);
        return row is null ? new UserSettingsResponse() : Map(row);
    }

    public async Task<UserSettingsResponse> UpdateAsync(Guid userId, UpdateUserSettingsRequest request, CancellationToken ct = default)
    {
        var theme = NormalizeTheme(request.Theme);

        var row = await _db.UserSettings.FirstOrDefaultAsync(s => s.UserId == userId, ct);
        if (row is null)
        {
            row = new UserSetting { UserId = userId };
            _db.UserSettings.Add(row);
        }

        row.NotificationSound = request.NotificationSound;
        row.CallSound = request.CallSound;
        row.DesktopNotifications = request.DesktopNotifications;
        row.ReadReceipts = request.ReadReceipts;
        row.Theme = theme;
        row.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return Map(row);
    }

    private static string NormalizeTheme(string? theme)
    {
        var value = (theme ?? string.Empty).Trim().ToLowerInvariant();
        return AllowedThemes.Contains(value) ? value : "system";
    }

    private static UserSettingsResponse Map(UserSetting s) => new()
    {
        NotificationSound = s.NotificationSound,
        CallSound = s.CallSound,
        DesktopNotifications = s.DesktopNotifications,
        ReadReceipts = s.ReadReceipts,
        Theme = s.Theme
    };
}

