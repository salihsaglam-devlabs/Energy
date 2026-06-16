using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.LocalizationResource.Services;
using Energy.Shared.Models.V1.Core.LocalizationResource.Requests;
using Energy.Shared.Models.V1.Core.LocalizationResource.Responses;

namespace Energy.Infrastructure.Modules.Core.LocalizationResource.Services;

/// <summary>LocalizationResource CRUD servisi (projection, pagination, soft-delete).</summary>
public class LocalizationResourceService : ILocalizationResourceService
{
    private readonly EnergyDbContext _db;

    public LocalizationResourceService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<LocalizationResourceListResponse>>> GetListAsync(GetLocalizationResourceListRequest request, CancellationToken ct = default)
    {
        var query = _db.LocalizationResources.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new LocalizationResourceListResponse
            {
                Id = e.Id,
                Key = e.Key,
                Culture = e.Culture,
                Value = e.Value,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<LocalizationResourceListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<LocalizationResourceListResponse>>.Success(page);
    }

    public async Task<BaseResponse<LocalizationResourceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.LocalizationResources.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new LocalizationResourceDetailResponse
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
                Culture = e.Culture,
                Value = e.Value
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<LocalizationResourceDetailResponse>.Failure("NotFound")
            : BaseResponse<LocalizationResourceDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateLocalizationResourceRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Core.LocalizationResource
        {
            Id = Guid.NewGuid(),
            Key = request.Key,
            Culture = request.Culture,
            Value = request.Value,
            CreatedAt = DateTime.UtcNow,
        };
        _db.LocalizationResources.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateLocalizationResourceRequest request, CancellationToken ct = default)
    {
        var entity = await _db.LocalizationResources.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Key = request.Key;
            entity.Culture = request.Culture;
            entity.Value = request.Value;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.LocalizationResources.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
