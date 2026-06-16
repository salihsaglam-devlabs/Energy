using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.WarehouseTransfer.Services;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Requests;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Responses;

namespace Energy.Infrastructure.Modules.Inventory.WarehouseTransfer.Services;

/// <summary>WarehouseTransfer CRUD servisi (projection, pagination, soft-delete).</summary>
public class WarehouseTransferService : IWarehouseTransferService
{
    private readonly EnergyDbContext _db;

    public WarehouseTransferService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<WarehouseTransferListResponse>>> GetListAsync(GetWarehouseTransferListRequest request, CancellationToken ct = default)
    {
        var query = _db.WarehouseTransfers.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new WarehouseTransferListResponse
            {
                Id = e.Id,
                SourceWarehouseId = e.SourceWarehouseId,
                TargetWarehouseId = e.TargetWarehouseId,
                TransferNo = e.TransferNo,
                TransferDate = e.TransferDate,
                Status = e.Status,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<WarehouseTransferListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<WarehouseTransferListResponse>>.Success(page);
    }

    public async Task<BaseResponse<WarehouseTransferDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.WarehouseTransfers.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new WarehouseTransferDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                SourceWarehouseId = e.SourceWarehouseId,
                TargetWarehouseId = e.TargetWarehouseId,
                TransferNo = e.TransferNo,
                TransferDate = e.TransferDate,
                Status = e.Status
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<WarehouseTransferDetailResponse>.Failure("NotFound")
            : BaseResponse<WarehouseTransferDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateWarehouseTransferRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Inventory.WarehouseTransfer
        {
            Id = Guid.NewGuid(),
            SourceWarehouseId = request.SourceWarehouseId,
            TargetWarehouseId = request.TargetWarehouseId,
            TransferNo = request.TransferNo,
            TransferDate = request.TransferDate,
            Status = request.Status,
            CreatedAt = DateTime.UtcNow,
        };
        _db.WarehouseTransfers.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWarehouseTransferRequest request, CancellationToken ct = default)
    {
        var entity = await _db.WarehouseTransfers.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.SourceWarehouseId = request.SourceWarehouseId;
            entity.TargetWarehouseId = request.TargetWarehouseId;
            entity.TransferNo = request.TransferNo;
            entity.TransferDate = request.TransferDate;
            entity.Status = request.Status;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.WarehouseTransfers.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
