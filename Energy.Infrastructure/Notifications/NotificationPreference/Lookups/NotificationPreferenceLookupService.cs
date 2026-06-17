using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Notifications.NotificationPreference.Lookups;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Responses;

namespace Energy.Infrastructure.Notifications.NotificationPreference.Lookups;

/// <summary>NotificationPreference lookup servisi (aktif + arama filtreli projection).</summary>
public class NotificationPreferenceLookupService : INotificationPreferenceLookupService
{
    private readonly AppDbContext _db;

    public NotificationPreferenceLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<NotificationPreferenceLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.NotificationPreferences.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<NotificationPreferenceLookupResponse>)rows.Select(e => new NotificationPreferenceLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.NotificationType ?? "")) ? "Notification Preference #" + e.Id.ToString().Substring(0, 8) : ((e.NotificationType ?? "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<NotificationPreferenceLookupResponse>>.Success(items);
    }
}
