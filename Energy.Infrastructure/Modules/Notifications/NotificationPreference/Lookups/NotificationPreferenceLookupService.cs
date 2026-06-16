using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Notifications.NotificationPreference.Lookups;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Responses;

namespace Energy.Infrastructure.Modules.Notifications.NotificationPreference.Lookups;

/// <summary>NotificationPreference lookup servisi (aktif + arama filtreli projection).</summary>
public class NotificationPreferenceLookupService : INotificationPreferenceLookupService
{
    private readonly AppDbContext _db;

    public NotificationPreferenceLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<NotificationPreferenceLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.NotificationPreferences.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new NotificationPreferenceLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<NotificationPreferenceLookupResponse>>.Success(items);
    }
}
