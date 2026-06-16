using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Catalog.Brand.Services;
using Energy.Shared.Models.V1.Catalog.Brand.Requests;
using Energy.Shared.Models.V1.Catalog.Brand.Responses;

namespace Energy.Infrastructure.Catalog.Brand.Services;

/// <summary>Brand CRUD servisi (projection, pagination, soft-delete).</summary>
public class BrandService : IBrandService
{
    private readonly AppDbContext _db;

    public BrandService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<BrandListResponse>>> GetListAsync(GetBrandListRequest request, CancellationToken ct = default)
    {
        var query = _db.Brands.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new BrandListResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<BrandListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<BrandListResponse>>.Success(page);
    }

    public async Task<BaseResponse<BrandDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Brands.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new BrandDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<BrandDetailResponse>.Failure("NotFound")
            : BaseResponse<BrandDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateBrandRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Catalog.Brand
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Brands.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBrandRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Brands.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Brands.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
