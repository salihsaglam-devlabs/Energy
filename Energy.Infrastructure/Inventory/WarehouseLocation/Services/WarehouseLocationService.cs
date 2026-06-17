using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Inventory.WarehouseLocation.Services;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Requests;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Responses;

namespace Energy.Infrastructure.Inventory.WarehouseLocation.Services;

/// <summary>WarehouseLocation CRUD servisi (projection, pagination, soft-delete).</summary>
public class WarehouseLocationService : IWarehouseLocationService
{
    private readonly AppDbContext _db;

    public WarehouseLocationService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<WarehouseLocationListResponse>>> GetListAsync(GetWarehouseLocationListRequest request, CancellationToken ct = default)
    {
        var query = _db.WarehouseLocations.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new WarehouseLocationListResponse
            {
                Id = e.Id,
                WarehouseId = e.WarehouseId,
                ParentLocationId = e.ParentLocationId,
                Code = e.Code,
                Name = e.Name,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<WarehouseLocationListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<WarehouseLocationListResponse>>.Success(page);
    }

    public async Task<BaseResponse<WarehouseLocationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.WarehouseLocations.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new WarehouseLocationDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                WarehouseId = e.WarehouseId,
                ParentLocationId = e.ParentLocationId,
                Code = e.Code,
                Name = e.Name
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<WarehouseLocationDetailResponse>.Failure("NotFound")
            : BaseResponse<WarehouseLocationDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateWarehouseLocationRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Inventory.WarehouseLocation
        {
            Id = Guid.NewGuid(),
            WarehouseId = request.WarehouseId,
            ParentLocationId = request.ParentLocationId,
            Code = request.Code,
            Name = request.Name,
            CreatedAt = DateTime.UtcNow,
        };
        _db.WarehouseLocations.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWarehouseLocationRequest request, CancellationToken ct = default)
    {
        var entity = await _db.WarehouseLocations.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.WarehouseId = request.WarehouseId;
            entity.ParentLocationId = request.ParentLocationId;
            entity.Code = request.Code;
            entity.Name = request.Name;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.WarehouseLocations.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
