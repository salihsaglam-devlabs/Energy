using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockLot.Services;
using Energy.Shared.Models.V1.Inventory.StockLot.Requests;
using Energy.Shared.Models.V1.Inventory.StockLot.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockLot.Services;

/// <summary>StockLot CRUD servisi (projection, pagination, soft-delete).</summary>
public class StockLotService : IStockLotService
{
    private readonly AppDbContext _db;

    public StockLotService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<StockLotListResponse>>> GetListAsync(GetStockLotListRequest request, CancellationToken ct = default)
    {
        var query = _db.StockLots.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new StockLotListResponse
            {
                Id = e.Id,
                WarehouseId = e.WarehouseId,
                MaterialId = e.MaterialId,
                SourceStockDocumentLineId = e.SourceStockDocumentLineId,
                LotNo = e.LotNo,
                InitialQuantity = e.InitialQuantity,
                RemainingQuantity = e.RemainingQuantity,
                UnitCost = e.UnitCost,
                ReceivedAt = e.ReceivedAt,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<StockLotListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<StockLotListResponse>>.Success(page);
    }

    public async Task<BaseResponse<StockLotDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.StockLots.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new StockLotDetailResponse
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
                SourceStockDocumentLineId = e.SourceStockDocumentLineId,
                LotNo = e.LotNo,
                InitialQuantity = e.InitialQuantity,
                RemainingQuantity = e.RemainingQuantity,
                UnitCost = e.UnitCost,
                ReceivedAt = e.ReceivedAt
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<StockLotDetailResponse>.Failure("NotFound")
            : BaseResponse<StockLotDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateStockLotRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Inventory.StockLot
        {
            Id = Guid.NewGuid(),
            WarehouseId = request.WarehouseId,
            MaterialId = request.MaterialId,
            SourceStockDocumentLineId = request.SourceStockDocumentLineId,
            LotNo = request.LotNo,
            InitialQuantity = request.InitialQuantity,
            RemainingQuantity = request.RemainingQuantity,
            UnitCost = request.UnitCost,
            ReceivedAt = request.ReceivedAt,
            CreatedAt = DateTime.UtcNow,
        };
        _db.StockLots.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockLotRequest request, CancellationToken ct = default)
    {
        var entity = await _db.StockLots.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.WarehouseId = request.WarehouseId;
            entity.MaterialId = request.MaterialId;
            entity.SourceStockDocumentLineId = request.SourceStockDocumentLineId;
            entity.LotNo = request.LotNo;
            entity.InitialQuantity = request.InitialQuantity;
            entity.RemainingQuantity = request.RemainingQuantity;
            entity.UnitCost = request.UnitCost;
            entity.ReceivedAt = request.ReceivedAt;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.StockLots.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
