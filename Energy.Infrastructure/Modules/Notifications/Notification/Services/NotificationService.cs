using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Notifications.Notification.Services;
using Energy.Shared.Models.V1.Notifications.Notification.Requests;
using Energy.Shared.Models.V1.Notifications.Notification.Responses;

namespace Energy.Infrastructure.Modules.Notifications.Notification.Services;

/// <summary>Notification CRUD servisi (projection, pagination, soft-delete).</summary>
public class NotificationService : INotificationService
{
    private readonly AppDbContext _db;

    public NotificationService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<NotificationListResponse>>> GetListAsync(GetNotificationListRequest request, CancellationToken ct = default)
    {
        var query = _db.Notifications.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new NotificationListResponse
            {
                Id = e.Id,
                Title = e.Title,
                Body = e.Body,
                NotificationType = e.NotificationType,
                RelatedModule = e.RelatedModule,
                RelatedEntityType = e.RelatedEntityType,
                RelatedEntityId = e.RelatedEntityId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<NotificationListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<NotificationListResponse>>.Success(page);
    }

    public async Task<BaseResponse<NotificationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Notifications.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new NotificationDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                Title = e.Title,
                Body = e.Body,
                NotificationType = e.NotificationType,
                RelatedModule = e.RelatedModule,
                RelatedEntityType = e.RelatedEntityType,
                RelatedEntityId = e.RelatedEntityId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<NotificationDetailResponse>.Failure("NotFound")
            : BaseResponse<NotificationDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateNotificationRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Notifications.Notification
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Body = request.Body,
            NotificationType = request.NotificationType,
            RelatedModule = request.RelatedModule,
            RelatedEntityType = request.RelatedEntityType,
            RelatedEntityId = request.RelatedEntityId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Notifications.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateNotificationRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Notifications.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Title = request.Title;
            entity.Body = request.Body;
            entity.NotificationType = request.NotificationType;
            entity.RelatedModule = request.RelatedModule;
            entity.RelatedEntityType = request.RelatedEntityType;
            entity.RelatedEntityId = request.RelatedEntityId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Notifications.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
