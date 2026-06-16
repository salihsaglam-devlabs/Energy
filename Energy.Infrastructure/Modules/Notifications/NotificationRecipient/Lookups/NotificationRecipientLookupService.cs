using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Notifications.NotificationRecipient.Lookups;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Responses;

namespace Energy.Infrastructure.Modules.Notifications.NotificationRecipient.Lookups;

/// <summary>NotificationRecipient lookup servisi (aktif + arama filtreli projection).</summary>
public class NotificationRecipientLookupService : INotificationRecipientLookupService
{
    private readonly AppDbContext _db;

    public NotificationRecipientLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<NotificationRecipientLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.NotificationRecipients.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new NotificationRecipientLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<NotificationRecipientLookupResponse>>.Success(items);
    }
}
