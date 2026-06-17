using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.IAM.UserSetting.Services;
using Energy.Shared.Models.V1.IAM.UserSetting.Requests;
using Energy.Shared.Models.V1.IAM.UserSetting.Responses;

namespace Energy.Infrastructure.IAM.UserSetting.Services;

/// <summary>
/// UserSetting: doğal/bileşik anahtarlı IAM kaydı. Liste/oluşturma desteklenir;
/// surrogate Guid ile yönetim parent/self-service ekranlarından yapılır.
/// </summary>
public class UserSettingService : IUserSettingService
{
    private readonly AppDbContext _db;

    public UserSettingService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<UserSettingListResponse>>> GetListAsync(GetUserSettingListRequest request, CancellationToken ct = default)
    {
        var query = _db.UserSettings.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(e => e.UserId)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new UserSettingListResponse
            {
                Id = Guid.Empty,
                UserId = e.UserId,
                NotificationSound = e.NotificationSound,
                CallSound = e.CallSound,
                DesktopNotifications = e.DesktopNotifications,
                ReadReceipts = e.ReadReceipts,
                Theme = e.Theme
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<UserSettingListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<UserSettingListResponse>>.Success(page);
    }

    public async Task<BaseResponse<UserSettingDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.UserSettings.AsNoTracking().Where(e => e.UserId == id)
            .Select(e => new UserSettingDetailResponse
            {
                Id = Guid.Empty,
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
        var entity = new global::Energy.Domain.IAM.UserSetting
        {
            UserId = request.UserId,
            NotificationSound = request.NotificationSound,
            CallSound = request.CallSound,
            DesktopNotifications = request.DesktopNotifications,
            ReadReceipts = request.ReadReceipts,
            Theme = request.Theme
        };
        _db.UserSettings.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(Guid.Empty, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateUserSettingRequest request, CancellationToken ct = default)
    {
        var entity = await _db.UserSettings.FirstOrDefaultAsync(e => e.UserId == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.NotificationSound = request.NotificationSound;
        entity.CallSound = request.CallSound;
        entity.DesktopNotifications = request.DesktopNotifications;
        entity.ReadReceipts = request.ReadReceipts;
        entity.Theme = request.Theme;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.UserSettings.FirstOrDefaultAsync(e => e.UserId == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        _db.UserSettings.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
