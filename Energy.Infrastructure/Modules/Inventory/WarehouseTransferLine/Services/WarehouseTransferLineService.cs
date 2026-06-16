using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.WarehouseTransferLine.Services;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Requests;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Responses;

namespace Energy.Infrastructure.Modules.Inventory.WarehouseTransferLine.Services;

/// <summary>WarehouseTransferLine CRUD servisi (projection, pagination, soft-delete).</summary>
public class WarehouseTransferLineService : IWarehouseTransferLineService
{
    private readonly EnergyDbContext _db;

    public WarehouseTransferLineService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<WarehouseTransferLineListResponse>>> GetListAsync(GetWarehouseTransferLineListRequest request, CancellationToken ct = default)
    {
        var query = _db.WarehouseTransferLines.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new WarehouseTransferLineListResponse
            {
                Id = e.Id,
                WarehouseTransferId = e.WarehouseTransferId,
                MaterialId = e.MaterialId,
                Quantity = e.Quantity,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<WarehouseTransferLineListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<WarehouseTransferLineListResponse>>.Success(page);
    }

    public async Task<BaseResponse<WarehouseTransferLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.WarehouseTransferLines.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new WarehouseTransferLineDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                WarehouseTransferId = e.WarehouseTransferId,
                MaterialId = e.MaterialId,
                Quantity = e.Quantity
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<WarehouseTransferLineDetailResponse>.Failure("NotFound")
            : BaseResponse<WarehouseTransferLineDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateWarehouseTransferLineRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Inventory.WarehouseTransferLine
        {
            Id = Guid.NewGuid(),
            WarehouseTransferId = request.WarehouseTransferId,
            MaterialId = request.MaterialId,
            Quantity = request.Quantity,
            CreatedAt = DateTime.UtcNow,
        };
        _db.WarehouseTransferLines.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWarehouseTransferLineRequest request, CancellationToken ct = default)
    {
        var entity = await _db.WarehouseTransferLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.WarehouseTransferId = request.WarehouseTransferId;
            entity.MaterialId = request.MaterialId;
            entity.Quantity = request.Quantity;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.WarehouseTransferLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
