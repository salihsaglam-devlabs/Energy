using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockTransaction.Services;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Requests;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockTransaction.Services;

/// <summary>StockTransaction CRUD servisi (projection, pagination, soft-delete).</summary>
public class StockTransactionService : IStockTransactionService
{
    private readonly EnergyDbContext _db;

    public StockTransactionService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<StockTransactionListResponse>>> GetListAsync(GetStockTransactionListRequest request, CancellationToken ct = default)
    {
        var query = _db.StockTransactions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new StockTransactionListResponse
            {
                Id = e.Id,
                StockDocumentId = e.StockDocumentId,
                StockDocumentLineId = e.StockDocumentLineId,
                StockLotId = e.StockLotId,
                WarehouseId = e.WarehouseId,
                MaterialId = e.MaterialId,
                Quantity = e.Quantity,
                UnitCost = e.UnitCost,
                TransactionDate = e.TransactionDate,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<StockTransactionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<StockTransactionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<StockTransactionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.StockTransactions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new StockTransactionDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                StockDocumentId = e.StockDocumentId,
                StockDocumentLineId = e.StockDocumentLineId,
                StockLotId = e.StockLotId,
                WarehouseId = e.WarehouseId,
                MaterialId = e.MaterialId,
                Quantity = e.Quantity,
                UnitCost = e.UnitCost,
                TransactionDate = e.TransactionDate
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<StockTransactionDetailResponse>.Failure("NotFound")
            : BaseResponse<StockTransactionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateStockTransactionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Inventory.StockTransaction
        {
            Id = Guid.NewGuid(),
            StockDocumentId = request.StockDocumentId,
            StockDocumentLineId = request.StockDocumentLineId,
            StockLotId = request.StockLotId,
            WarehouseId = request.WarehouseId,
            MaterialId = request.MaterialId,
            Quantity = request.Quantity,
            UnitCost = request.UnitCost,
            TransactionDate = request.TransactionDate,
            CreatedAt = DateTime.UtcNow,
        };
        _db.StockTransactions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockTransactionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.StockTransactions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.StockDocumentId = request.StockDocumentId;
            entity.StockDocumentLineId = request.StockDocumentLineId;
            entity.StockLotId = request.StockLotId;
            entity.WarehouseId = request.WarehouseId;
            entity.MaterialId = request.MaterialId;
            entity.Quantity = request.Quantity;
            entity.UnitCost = request.UnitCost;
            entity.TransactionDate = request.TransactionDate;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.StockTransactions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
