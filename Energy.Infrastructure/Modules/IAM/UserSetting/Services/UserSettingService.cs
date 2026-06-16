using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.UserSetting.Services;
using Energy.Shared.Models.V1.IAM.UserSetting.Requests;
using Energy.Shared.Models.V1.IAM.UserSetting.Responses;

namespace Energy.Infrastructure.Modules.IAM.UserSetting.Services;

/// <summary>UserSetting CRUD servisi (projection, pagination, soft-delete).</summary>
public class UserSettingService : IUserSettingService
{
    private readonly EnergyDbContext _db;

    public UserSettingService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<UserSettingListResponse>>> GetListAsync(GetUserSettingListRequest request, CancellationToken ct = default)
    {
        var query = _db.UserSettings.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new UserSettingListResponse
            {
                Id = e.Id,
                UserId = e.UserId,
                NotificationSound = e.NotificationSound,
                CallSound = e.CallSound,
                DesktopNotifications = e.DesktopNotifications,
                ReadReceipts = e.ReadReceipts,
                Theme = e.Theme,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<UserSettingListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<UserSettingListResponse>>.Success(page);
    }

    public async Task<BaseResponse<UserSettingDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.UserSettings.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new UserSettingDetailResponse
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
                NotificationSound = e.NotificationSound,
                CallSound = e.CallSound,
                DesktopNotifications = e.DesktopNotifications,
                ReadReceipts = e.ReadReceipts,
                Theme = e.Theme
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<UserSettingDetailResponse>.Failure("NotFound")
            : BaseResponse<UserSettingDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateUserSettingRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.IAM.UserSetting
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            NotificationSound = request.NotificationSound,
            CallSound = request.CallSound,
            DesktopNotifications = request.DesktopNotifications,
            ReadReceipts = request.ReadReceipts,
            Theme = request.Theme,
            CreatedAt = DateTime.UtcNow,
        };
        _db.UserSettings.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateUserSettingRequest request, CancellationToken ct = default)
    {
        var entity = await _db.UserSettings.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.UserId = request.UserId;
            entity.NotificationSound = request.NotificationSound;
            entity.CallSound = request.CallSound;
            entity.DesktopNotifications = request.DesktopNotifications;
            entity.ReadReceipts = request.ReadReceipts;
            entity.Theme = request.Theme;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.UserSettings.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
