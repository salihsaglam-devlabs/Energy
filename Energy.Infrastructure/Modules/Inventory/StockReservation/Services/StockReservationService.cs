using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockReservation.Services;
using Energy.Shared.Models.V1.Inventory.StockReservation.Requests;
using Energy.Shared.Models.V1.Inventory.StockReservation.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockReservation.Services;

/// <summary>StockReservation CRUD servisi (projection, pagination, soft-delete).</summary>
public class StockReservationService : IStockReservationService
{
    private readonly AppDbContext _db;

    public StockReservationService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<StockReservationListResponse>>> GetListAsync(GetStockReservationListRequest request, CancellationToken ct = default)
    {
        var query = _db.StockReservations.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new StockReservationListResponse
            {
                Id = e.Id,
                WarehouseId = e.WarehouseId,
                MaterialId = e.MaterialId,
                Quantity = e.Quantity,
                RelatedModule = e.RelatedModule,
                RelatedEntityType = e.RelatedEntityType,
                RelatedEntityId = e.RelatedEntityId,
                IsReleased = e.IsReleased,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<StockReservationListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<StockReservationListResponse>>.Success(page);
    }

    public async Task<BaseResponse<StockReservationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.StockReservations.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new StockReservationDetailResponse
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
                RelatedModule = e.RelatedModule,
                RelatedEntityType = e.RelatedEntityType,
                RelatedEntityId = e.RelatedEntityId,
                IsReleased = e.IsReleased
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<StockReservationDetailResponse>.Failure("NotFound")
            : BaseResponse<StockReservationDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateStockReservationRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Inventory.StockReservation
        {
            Id = Guid.NewGuid(),
            WarehouseId = request.WarehouseId,
            MaterialId = request.MaterialId,
            Quantity = request.Quantity,
            RelatedModule = request.RelatedModule,
            RelatedEntityType = request.RelatedEntityType,
            RelatedEntityId = request.RelatedEntityId,
            IsReleased = request.IsReleased,
            CreatedAt = DateTime.UtcNow,
        };
        _db.StockReservations.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockReservationRequest request, CancellationToken ct = default)
    {
        var entity = await _db.StockReservations.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.WarehouseId = request.WarehouseId;
            entity.MaterialId = request.MaterialId;
            entity.Quantity = request.Quantity;
            entity.RelatedModule = request.RelatedModule;
            entity.RelatedEntityType = request.RelatedEntityType;
            entity.RelatedEntityId = request.RelatedEntityId;
            entity.IsReleased = request.IsReleased;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.StockReservations.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
