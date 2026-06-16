using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Procurement.PurchaseReceiptLine.Services;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Responses;

namespace Energy.Infrastructure.Modules.Procurement.PurchaseReceiptLine.Services;

/// <summary>PurchaseReceiptLine CRUD servisi (projection, pagination, soft-delete).</summary>
public class PurchaseReceiptLineService : IPurchaseReceiptLineService
{
    private readonly AppDbContext _db;

    public PurchaseReceiptLineService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<PurchaseReceiptLineListResponse>>> GetListAsync(GetPurchaseReceiptLineListRequest request, CancellationToken ct = default)
    {
        var query = _db.PurchaseReceiptLines.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new PurchaseReceiptLineListResponse
            {
                Id = e.Id,
                PurchaseReceiptId = e.PurchaseReceiptId,
                PurchaseOrderLineId = e.PurchaseOrderLineId,
                MaterialId = e.MaterialId,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<PurchaseReceiptLineListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<PurchaseReceiptLineListResponse>>.Success(page);
    }

    public async Task<BaseResponse<PurchaseReceiptLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.PurchaseReceiptLines.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new PurchaseReceiptLineDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                PurchaseReceiptId = e.PurchaseReceiptId,
                PurchaseOrderLineId = e.PurchaseOrderLineId,
                MaterialId = e.MaterialId,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<PurchaseReceiptLineDetailResponse>.Failure("NotFound")
            : BaseResponse<PurchaseReceiptLineDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreatePurchaseReceiptLineRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Procurement.PurchaseReceiptLine
        {
            Id = Guid.NewGuid(),
            PurchaseReceiptId = request.PurchaseReceiptId,
            PurchaseOrderLineId = request.PurchaseOrderLineId,
            MaterialId = request.MaterialId,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            CreatedAt = DateTime.UtcNow,
        };
        _db.PurchaseReceiptLines.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePurchaseReceiptLineRequest request, CancellationToken ct = default)
    {
        var entity = await _db.PurchaseReceiptLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.PurchaseReceiptId = request.PurchaseReceiptId;
            entity.PurchaseOrderLineId = request.PurchaseOrderLineId;
            entity.MaterialId = request.MaterialId;
            entity.Quantity = request.Quantity;
            entity.UnitPrice = request.UnitPrice;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.PurchaseReceiptLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
