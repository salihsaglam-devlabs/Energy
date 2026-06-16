using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockIssueAllocation.Services;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Requests;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockIssueAllocation.Services;

/// <summary>StockIssueAllocation CRUD servisi (projection, pagination, soft-delete).</summary>
public class StockIssueAllocationService : IStockIssueAllocationService
{
    private readonly AppDbContext _db;

    public StockIssueAllocationService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<StockIssueAllocationListResponse>>> GetListAsync(GetStockIssueAllocationListRequest request, CancellationToken ct = default)
    {
        var query = _db.StockIssueAllocations.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new StockIssueAllocationListResponse
            {
                Id = e.Id,
                StockDocumentLineId = e.StockDocumentLineId,
                StockLotId = e.StockLotId,
                Quantity = e.Quantity,
                UnitCost = e.UnitCost,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<StockIssueAllocationListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<StockIssueAllocationListResponse>>.Success(page);
    }

    public async Task<BaseResponse<StockIssueAllocationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.StockIssueAllocations.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new StockIssueAllocationDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                StockDocumentLineId = e.StockDocumentLineId,
                StockLotId = e.StockLotId,
                Quantity = e.Quantity,
                UnitCost = e.UnitCost
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<StockIssueAllocationDetailResponse>.Failure("NotFound")
            : BaseResponse<StockIssueAllocationDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateStockIssueAllocationRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Inventory.StockIssueAllocation
        {
            Id = Guid.NewGuid(),
            StockDocumentLineId = request.StockDocumentLineId,
            StockLotId = request.StockLotId,
            Quantity = request.Quantity,
            UnitCost = request.UnitCost,
            CreatedAt = DateTime.UtcNow,
        };
        _db.StockIssueAllocations.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockIssueAllocationRequest request, CancellationToken ct = default)
    {
        var entity = await _db.StockIssueAllocations.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.StockDocumentLineId = request.StockDocumentLineId;
            entity.StockLotId = request.StockLotId;
            entity.Quantity = request.Quantity;
            entity.UnitCost = request.UnitCost;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.StockIssueAllocations.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
