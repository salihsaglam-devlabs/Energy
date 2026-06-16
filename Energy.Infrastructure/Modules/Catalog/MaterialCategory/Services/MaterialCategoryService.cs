using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Catalog.MaterialCategory.Services;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Responses;

namespace Energy.Infrastructure.Modules.Catalog.MaterialCategory.Services;

/// <summary>MaterialCategory CRUD servisi (projection, pagination, soft-delete).</summary>
public class MaterialCategoryService : IMaterialCategoryService
{
    private readonly EnergyDbContext _db;

    public MaterialCategoryService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<MaterialCategoryListResponse>>> GetListAsync(GetMaterialCategoryListRequest request, CancellationToken ct = default)
    {
        var query = _db.MaterialCategories.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new MaterialCategoryListResponse
            {
                Id = e.Id,
                ParentCategoryId = e.ParentCategoryId,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<MaterialCategoryListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<MaterialCategoryListResponse>>.Success(page);
    }

    public async Task<BaseResponse<MaterialCategoryDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.MaterialCategories.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new MaterialCategoryDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ParentCategoryId = e.ParentCategoryId,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<MaterialCategoryDetailResponse>.Failure("NotFound")
            : BaseResponse<MaterialCategoryDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateMaterialCategoryRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Catalog.MaterialCategory
        {
            Id = Guid.NewGuid(),
            ParentCategoryId = request.ParentCategoryId,
            Code = request.Code,
            Name = request.Name,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.MaterialCategories.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialCategoryRequest request, CancellationToken ct = default)
    {
        var entity = await _db.MaterialCategories.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ParentCategoryId = request.ParentCategoryId;
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.MaterialCategories.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
