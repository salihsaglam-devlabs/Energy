using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockCountLine.Services;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Requests;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockCountLine.Services;

/// <summary>StockCountLine CRUD servisi (projection, pagination, soft-delete).</summary>
public class StockCountLineService : IStockCountLineService
{
    private readonly EnergyDbContext _db;

    public StockCountLineService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<StockCountLineListResponse>>> GetListAsync(GetStockCountLineListRequest request, CancellationToken ct = default)
    {
        var query = _db.StockCountLines.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new StockCountLineListResponse
            {
                Id = e.Id,
                StockCountId = e.StockCountId,
                MaterialId = e.MaterialId,
                SystemQuantity = e.SystemQuantity,
                CountedQuantity = e.CountedQuantity,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<StockCountLineListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<StockCountLineListResponse>>.Success(page);
    }

    public async Task<BaseResponse<StockCountLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.StockCountLines.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new StockCountLineDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                StockCountId = e.StockCountId,
                MaterialId = e.MaterialId,
                SystemQuantity = e.SystemQuantity,
                CountedQuantity = e.CountedQuantity
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<StockCountLineDetailResponse>.Failure("NotFound")
            : BaseResponse<StockCountLineDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateStockCountLineRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Inventory.StockCountLine
        {
            Id = Guid.NewGuid(),
            StockCountId = request.StockCountId,
            MaterialId = request.MaterialId,
            SystemQuantity = request.SystemQuantity,
            CountedQuantity = request.CountedQuantity,
            CreatedAt = DateTime.UtcNow,
        };
        _db.StockCountLines.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockCountLineRequest request, CancellationToken ct = default)
    {
        var entity = await _db.StockCountLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.StockCountId = request.StockCountId;
            entity.MaterialId = request.MaterialId;
            entity.SystemQuantity = request.SystemQuantity;
            entity.CountedQuantity = request.CountedQuantity;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.StockCountLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
