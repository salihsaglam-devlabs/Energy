using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Catalog.MaterialUnitConversion.Services;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Responses;

namespace Energy.Infrastructure.Modules.Catalog.MaterialUnitConversion.Services;

/// <summary>MaterialUnitConversion CRUD servisi (projection, pagination, soft-delete).</summary>
public class MaterialUnitConversionService : IMaterialUnitConversionService
{
    private readonly EnergyDbContext _db;

    public MaterialUnitConversionService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<MaterialUnitConversionListResponse>>> GetListAsync(GetMaterialUnitConversionListRequest request, CancellationToken ct = default)
    {
        var query = _db.MaterialUnitConversions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new MaterialUnitConversionListResponse
            {
                Id = e.Id,
                MaterialId = e.MaterialId,
                FromUnitOfMeasureId = e.FromUnitOfMeasureId,
                ToUnitOfMeasureId = e.ToUnitOfMeasureId,
                Factor = e.Factor,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<MaterialUnitConversionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<MaterialUnitConversionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<MaterialUnitConversionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.MaterialUnitConversions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new MaterialUnitConversionDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                MaterialId = e.MaterialId,
                FromUnitOfMeasureId = e.FromUnitOfMeasureId,
                ToUnitOfMeasureId = e.ToUnitOfMeasureId,
                Factor = e.Factor
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<MaterialUnitConversionDetailResponse>.Failure("NotFound")
            : BaseResponse<MaterialUnitConversionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateMaterialUnitConversionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Catalog.MaterialUnitConversion
        {
            Id = Guid.NewGuid(),
            MaterialId = request.MaterialId,
            FromUnitOfMeasureId = request.FromUnitOfMeasureId,
            ToUnitOfMeasureId = request.ToUnitOfMeasureId,
            Factor = request.Factor,
            CreatedAt = DateTime.UtcNow,
        };
        _db.MaterialUnitConversions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialUnitConversionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.MaterialUnitConversions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.MaterialId = request.MaterialId;
            entity.FromUnitOfMeasureId = request.FromUnitOfMeasureId;
            entity.ToUnitOfMeasureId = request.ToUnitOfMeasureId;
            entity.Factor = request.Factor;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.MaterialUnitConversions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
