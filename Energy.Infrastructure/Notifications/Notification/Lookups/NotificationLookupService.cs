using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Notifications.Notification.Lookups;
using Energy.Shared.Models.V1.Notifications.Notification.Responses;

namespace Energy.Infrastructure.Notifications.Notification.Lookups;

/// <summary>Notification lookup servisi (aktif + arama filtreli projection).</summary>
public class NotificationLookupService : INotificationLookupService
{
    private readonly AppDbContext _db;

    public NotificationLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<NotificationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Notifications.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Title.Contains(search));
        var items = await query
            .OrderBy(e => e.Title)
            .Select(e => new NotificationLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.Title,
                DisplayName = e.Title,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<NotificationLookupResponse>>.Success(items);
    }
}
