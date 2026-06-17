using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Notifications.NotificationRecipient.Lookups;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Responses;

namespace Energy.Infrastructure.Notifications.NotificationRecipient.Lookups;

/// <summary>NotificationRecipient lookup servisi (aktif + arama filtreli projection).</summary>
public class NotificationRecipientLookupService : INotificationRecipientLookupService
{
    private readonly AppDbContext _db;

    public NotificationRecipientLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<NotificationRecipientLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.NotificationRecipients.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<NotificationRecipientLookupResponse>)rows.Select(e => new NotificationRecipientLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.ReadAt.HasValue ? e.ReadAt.Value.ToString("yyyy-MM-dd") : "")) ? "Notification Recipient #" + e.Id.ToString().Substring(0, 8) : ((e.ReadAt.HasValue ? e.ReadAt.Value.ToString("yyyy-MM-dd") : "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<NotificationRecipientLookupResponse>>.Success(items);
    }
}
