using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Notifications.NotificationRecipient.Services;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Requests;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Responses;

namespace Energy.Infrastructure.Modules.Notifications.NotificationRecipient.Services;

/// <summary>NotificationRecipient CRUD servisi (projection, pagination, soft-delete).</summary>
public class NotificationRecipientService : INotificationRecipientService
{
    private readonly EnergyDbContext _db;

    public NotificationRecipientService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<NotificationRecipientListResponse>>> GetListAsync(GetNotificationRecipientListRequest request, CancellationToken ct = default)
    {
        var query = _db.NotificationRecipients.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new NotificationRecipientListResponse
            {
                Id = e.Id,
                NotificationId = e.NotificationId,
                UserId = e.UserId,
                IsRead = e.IsRead,
                ReadAt = e.ReadAt,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<NotificationRecipientListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<NotificationRecipientListResponse>>.Success(page);
    }

    public async Task<BaseResponse<NotificationRecipientDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.NotificationRecipients.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new NotificationRecipientDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                NotificationId = e.NotificationId,
                UserId = e.UserId,
                IsRead = e.IsRead,
                ReadAt = e.ReadAt
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<NotificationRecipientDetailResponse>.Failure("NotFound")
            : BaseResponse<NotificationRecipientDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateNotificationRecipientRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Notifications.NotificationRecipient
        {
            Id = Guid.NewGuid(),
            NotificationId = request.NotificationId,
            UserId = request.UserId,
            IsRead = request.IsRead,
            ReadAt = request.ReadAt,
            CreatedAt = DateTime.UtcNow,
        };
        _db.NotificationRecipients.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateNotificationRecipientRequest request, CancellationToken ct = default)
    {
        var entity = await _db.NotificationRecipients.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.NotificationId = request.NotificationId;
            entity.UserId = request.UserId;
            entity.IsRead = request.IsRead;
            entity.ReadAt = request.ReadAt;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.NotificationRecipients.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
