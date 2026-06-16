using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockBalance.Services;
using Energy.Shared.Models.V1.Inventory.StockBalance.Requests;
using Energy.Shared.Models.V1.Inventory.StockBalance.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockBalance.Services;

/// <summary>StockBalance CRUD servisi (projection, pagination, soft-delete).</summary>
public class StockBalanceService : IStockBalanceService
{
    private readonly AppDbContext _db;

    public StockBalanceService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<StockBalanceListResponse>>> GetListAsync(GetStockBalanceListRequest request, CancellationToken ct = default)
    {
        var query = _db.StockBalances.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new StockBalanceListResponse
            {
                Id = e.Id,
                WarehouseId = e.WarehouseId,
                MaterialId = e.MaterialId,
                Quantity = e.Quantity,
                ReservedQuantity = e.ReservedQuantity,
                TotalCost = e.TotalCost,
                LastRecalculatedAt = e.LastRecalculatedAt,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<StockBalanceListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<StockBalanceListResponse>>.Success(page);
    }

    public async Task<BaseResponse<StockBalanceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.StockBalances.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new StockBalanceDetailResponse
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
                MaterialId = e.MaterialId,
                Quantity = e.Quantity,
                ReservedQuantity = e.ReservedQuantity,
                TotalCost = e.TotalCost,
                LastRecalculatedAt = e.LastRecalculatedAt
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<StockBalanceDetailResponse>.Failure("NotFound")
            : BaseResponse<StockBalanceDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateStockBalanceRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Inventory.StockBalance
        {
            Id = Guid.NewGuid(),
            WarehouseId = request.WarehouseId,
            MaterialId = request.MaterialId,
            Quantity = request.Quantity,
            ReservedQuantity = request.ReservedQuantity,
            TotalCost = request.TotalCost,
            LastRecalculatedAt = request.LastRecalculatedAt,
            CreatedAt = DateTime.UtcNow,
        };
        _db.StockBalances.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockBalanceRequest request, CancellationToken ct = default)
    {
        var entity = await _db.StockBalances.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.WarehouseId = request.WarehouseId;
            entity.MaterialId = request.MaterialId;
            entity.Quantity = request.Quantity;
            entity.ReservedQuantity = request.ReservedQuantity;
            entity.TotalCost = request.TotalCost;
            entity.LastRecalculatedAt = request.LastRecalculatedAt;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.StockBalances.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
