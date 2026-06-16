using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Procurement.PurchaseOrderLine.Services;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Responses;

namespace Energy.Infrastructure.Procurement.PurchaseOrderLine.Services;

/// <summary>PurchaseOrderLine CRUD servisi (projection, pagination, soft-delete).</summary>
public class PurchaseOrderLineService : IPurchaseOrderLineService
{
    private readonly AppDbContext _db;

    public PurchaseOrderLineService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<PurchaseOrderLineListResponse>>> GetListAsync(GetPurchaseOrderLineListRequest request, CancellationToken ct = default)
    {
        var query = _db.PurchaseOrderLines.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new PurchaseOrderLineListResponse
            {
                Id = e.Id,
                PurchaseOrderId = e.PurchaseOrderId,
                RequestLineId = e.RequestLineId,
                MaterialId = e.MaterialId,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice,
                CurrencyId = e.CurrencyId,
                ReceivedQuantity = e.ReceivedQuantity,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<PurchaseOrderLineListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<PurchaseOrderLineListResponse>>.Success(page);
    }

    public async Task<BaseResponse<PurchaseOrderLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.PurchaseOrderLines.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new PurchaseOrderLineDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                PurchaseOrderId = e.PurchaseOrderId,
                RequestLineId = e.RequestLineId,
                MaterialId = e.MaterialId,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice,
                CurrencyId = e.CurrencyId,
                ReceivedQuantity = e.ReceivedQuantity
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<PurchaseOrderLineDetailResponse>.Failure("NotFound")
            : BaseResponse<PurchaseOrderLineDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreatePurchaseOrderLineRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Procurement.PurchaseOrderLine
        {
            Id = Guid.NewGuid(),
            PurchaseOrderId = request.PurchaseOrderId,
            RequestLineId = request.RequestLineId,
            MaterialId = request.MaterialId,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            CurrencyId = request.CurrencyId,
            ReceivedQuantity = request.ReceivedQuantity,
            CreatedAt = DateTime.UtcNow,
        };
        _db.PurchaseOrderLines.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePurchaseOrderLineRequest request, CancellationToken ct = default)
    {
        var entity = await _db.PurchaseOrderLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.PurchaseOrderId = request.PurchaseOrderId;
            entity.RequestLineId = request.RequestLineId;
            entity.MaterialId = request.MaterialId;
            entity.Quantity = request.Quantity;
            entity.UnitPrice = request.UnitPrice;
            entity.CurrencyId = request.CurrencyId;
            entity.ReceivedQuantity = request.ReceivedQuantity;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.PurchaseOrderLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
