using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Catalog.Material.Services;
using Energy.Shared.Models.V1.Catalog.Material.Requests;
using Energy.Shared.Models.V1.Catalog.Material.Responses;

namespace Energy.Infrastructure.Modules.Catalog.Material.Services;

/// <summary>Material CRUD servisi (projection, pagination, soft-delete).</summary>
public class MaterialService : IMaterialService
{
    private readonly EnergyDbContext _db;

    public MaterialService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<MaterialListResponse>>> GetListAsync(GetMaterialListRequest request, CancellationToken ct = default)
    {
        var query = _db.Materials.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new MaterialListResponse
            {
                Id = e.Id,
                MaterialCategoryId = e.MaterialCategoryId,
                BrandId = e.BrandId,
                BaseUnitOfMeasureId = e.BaseUnitOfMeasureId,
                Code = e.Code,
                Name = e.Name,
                IsBatchTracked = e.IsBatchTracked,
                IsSerialTracked = e.IsSerialTracked,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<MaterialListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<MaterialListResponse>>.Success(page);
    }

    public async Task<BaseResponse<MaterialDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Materials.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new MaterialDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                MaterialCategoryId = e.MaterialCategoryId,
                BrandId = e.BrandId,
                BaseUnitOfMeasureId = e.BaseUnitOfMeasureId,
                Code = e.Code,
                Name = e.Name,
                IsBatchTracked = e.IsBatchTracked,
                IsSerialTracked = e.IsSerialTracked,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<MaterialDetailResponse>.Failure("NotFound")
            : BaseResponse<MaterialDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateMaterialRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Catalog.Material
        {
            Id = Guid.NewGuid(),
            MaterialCategoryId = request.MaterialCategoryId,
            BrandId = request.BrandId,
            BaseUnitOfMeasureId = request.BaseUnitOfMeasureId,
            Code = request.Code,
            Name = request.Name,
            IsBatchTracked = request.IsBatchTracked,
            IsSerialTracked = request.IsSerialTracked,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Materials.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Materials.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.MaterialCategoryId = request.MaterialCategoryId;
            entity.BrandId = request.BrandId;
            entity.BaseUnitOfMeasureId = request.BaseUnitOfMeasureId;
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.IsBatchTracked = request.IsBatchTracked;
            entity.IsSerialTracked = request.IsSerialTracked;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Materials.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
