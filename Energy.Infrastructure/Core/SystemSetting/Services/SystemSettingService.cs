using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Core.SystemSetting.Services;
using Energy.Shared.Models.V1.Core.SystemSetting.Requests;
using Energy.Shared.Models.V1.Core.SystemSetting.Responses;

namespace Energy.Infrastructure.Core.SystemSetting.Services;

/// <summary>SystemSetting CRUD servisi (projection, pagination, soft-delete).</summary>
public class SystemSettingService : ISystemSettingService
{
    private readonly AppDbContext _db;

    public SystemSettingService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<SystemSettingListResponse>>> GetListAsync(GetSystemSettingListRequest request, CancellationToken ct = default)
    {
        var query = _db.SystemSettings.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new SystemSettingListResponse
            {
                Id = e.Id,
                Key = e.Key,
                Value = e.Value,
                Category = e.Category,
                DescriptionKey = e.DescriptionKey,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<SystemSettingListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<SystemSettingListResponse>>.Success(page);
    }

    public async Task<BaseResponse<SystemSettingDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.SystemSettings.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new SystemSettingDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                Key = e.Key,
                Value = e.Value,
                Category = e.Category,
                DescriptionKey = e.DescriptionKey
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<SystemSettingDetailResponse>.Failure("NotFound")
            : BaseResponse<SystemSettingDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateSystemSettingRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Core.SystemSetting
        {
            Id = Guid.NewGuid(),
            Key = request.Key,
            Value = request.Value,
            Category = request.Category,
            DescriptionKey = request.DescriptionKey,
            CreatedAt = DateTime.UtcNow,
        };
        _db.SystemSettings.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSystemSettingRequest request, CancellationToken ct = default)
    {
        var entity = await _db.SystemSettings.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Key = request.Key;
            entity.Value = request.Value;
            entity.Category = request.Category;
            entity.DescriptionKey = request.DescriptionKey;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.SystemSettings.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
