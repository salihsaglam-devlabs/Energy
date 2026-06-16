using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Inventory.StockCount.Services;
using Energy.Shared.Models.V1.Inventory.StockCount.Requests;
using Energy.Shared.Models.V1.Inventory.StockCount.Responses;

namespace Energy.Infrastructure.Inventory.StockCount.Services;

/// <summary>StockCount CRUD servisi (projection, pagination, soft-delete).</summary>
public class StockCountService : IStockCountService
{
    private readonly AppDbContext _db;

    public StockCountService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<StockCountListResponse>>> GetListAsync(GetStockCountListRequest request, CancellationToken ct = default)
    {
        var query = _db.StockCounts.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new StockCountListResponse
            {
                Id = e.Id,
                WarehouseId = e.WarehouseId,
                CountNo = e.CountNo,
                CountDate = e.CountDate,
                Status = e.Status,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<StockCountListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<StockCountListResponse>>.Success(page);
    }

    public async Task<BaseResponse<StockCountDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.StockCounts.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new StockCountDetailResponse
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
                CountNo = e.CountNo,
                CountDate = e.CountDate,
                Status = e.Status
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<StockCountDetailResponse>.Failure("NotFound")
            : BaseResponse<StockCountDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateStockCountRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Inventory.StockCount
        {
            Id = Guid.NewGuid(),
            WarehouseId = request.WarehouseId,
            CountNo = request.CountNo,
            CountDate = request.CountDate,
            Status = request.Status,
            CreatedAt = DateTime.UtcNow,
        };
        _db.StockCounts.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockCountRequest request, CancellationToken ct = default)
    {
        var entity = await _db.StockCounts.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.WarehouseId = request.WarehouseId;
            entity.CountNo = request.CountNo;
            entity.CountDate = request.CountDate;
            entity.Status = request.Status;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.StockCounts.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
