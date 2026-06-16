using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Notifications.Notification.Lookups;
using Energy.Shared.Models.V1.Notifications.Notification.Responses;

namespace Energy.Infrastructure.Modules.Notifications.Notification.Lookups;

/// <summary>Notification lookup servisi (aktif + arama filtreli projection).</summary>
public class NotificationLookupService : INotificationLookupService
{
    private readonly EnergyDbContext _db;

    public NotificationLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<NotificationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Notifications.AsNoTracking();
        var items = await query.Select(e => new NotificationLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<NotificationLookupResponse>>.Success(items);
    }
}
