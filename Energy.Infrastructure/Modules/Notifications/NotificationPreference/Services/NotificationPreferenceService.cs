using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Notifications.NotificationPreference.Services;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Requests;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Responses;

namespace Energy.Infrastructure.Modules.Notifications.NotificationPreference.Services;

/// <summary>NotificationPreference CRUD servisi (projection, pagination, soft-delete).</summary>
public class NotificationPreferenceService : INotificationPreferenceService
{
    private readonly EnergyDbContext _db;

    public NotificationPreferenceService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<NotificationPreferenceListResponse>>> GetListAsync(GetNotificationPreferenceListRequest request, CancellationToken ct = default)
    {
        var query = _db.NotificationPreferences.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new NotificationPreferenceListResponse
            {
                Id = e.Id,
                UserId = e.UserId,
                NotificationType = e.NotificationType,
                InAppEnabled = e.InAppEnabled,
                EmailEnabled = e.EmailEnabled,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<NotificationPreferenceListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<NotificationPreferenceListResponse>>.Success(page);
    }

    public async Task<BaseResponse<NotificationPreferenceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.NotificationPreferences.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new NotificationPreferenceDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                UserId = e.UserId,
                NotificationType = e.NotificationType,
                InAppEnabled = e.InAppEnabled,
                EmailEnabled = e.EmailEnabled
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<NotificationPreferenceDetailResponse>.Failure("NotFound")
            : BaseResponse<NotificationPreferenceDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateNotificationPreferenceRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Notifications.NotificationPreference
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            NotificationType = request.NotificationType,
            InAppEnabled = request.InAppEnabled,
            EmailEnabled = request.EmailEnabled,
            CreatedAt = DateTime.UtcNow,
        };
        _db.NotificationPreferences.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateNotificationPreferenceRequest request, CancellationToken ct = default)
    {
        var entity = await _db.NotificationPreferences.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.UserId = request.UserId;
            entity.NotificationType = request.NotificationType;
            entity.InAppEnabled = request.InAppEnabled;
            entity.EmailEnabled = request.EmailEnabled;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.NotificationPreferences.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
