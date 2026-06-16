using Energy.Shared.Common;
using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.Warehouse.Services;
using Energy.Shared.Models.V1.Inventory.Warehouse.Requests;
using Energy.Shared.Models.V1.Inventory.Warehouse.Responses;

namespace Energy.Infrastructure.Modules.Inventory.Warehouse.Services;

/// <summary>Warehouse CRUD servisi (projection, pagination, soft-delete).</summary>
public class WarehouseService : IWarehouseService
{
    private readonly AppDbContext _db;

    public WarehouseService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<WarehouseListResponse>>> GetListAsync(GetWarehouseListRequest request, CancellationToken ct = default)
    {
        var query = _db.Warehouses.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new WarehouseListResponse
            {
                Id = e.Id,
                CompanyId = e.CompanyId,
                BranchId = e.BranchId,
                ProjectId = e.ProjectId,
                WarehouseType = e.WarehouseType,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<WarehouseListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<WarehouseListResponse>>.Success(page);
    }

    public async Task<BaseResponse<WarehouseDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Warehouses.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new WarehouseDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                CompanyId = e.CompanyId,
                BranchId = e.BranchId,
                ProjectId = e.ProjectId,
                WarehouseType = e.WarehouseType,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<WarehouseDetailResponse>.Failure("NotFound")
            : BaseResponse<WarehouseDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateWarehouseRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Inventory.Warehouse
        {
            Id = Guid.NewGuid(),
            CompanyId = request.CompanyId,
            BranchId = request.BranchId,
            ProjectId = request.ProjectId,
            WarehouseType = request.WarehouseType,
            Code = request.Code,
            Name = request.Name,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Warehouses.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWarehouseRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Warehouses.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.CompanyId = request.CompanyId;
            entity.BranchId = request.BranchId;
            entity.ProjectId = request.ProjectId;
            entity.WarehouseType = request.WarehouseType;
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Warehouses.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
