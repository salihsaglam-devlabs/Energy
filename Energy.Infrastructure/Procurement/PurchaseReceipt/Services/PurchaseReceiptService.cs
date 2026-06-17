using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Procurement.PurchaseReceipt.Services;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Responses;

namespace Energy.Infrastructure.Procurement.PurchaseReceipt.Services;

/// <summary>PurchaseReceipt CRUD servisi (projection, pagination, soft-delete).</summary>
public class PurchaseReceiptService : IPurchaseReceiptService
{
    private readonly AppDbContext _db;

    public PurchaseReceiptService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<PurchaseReceiptListResponse>>> GetListAsync(GetPurchaseReceiptListRequest request, CancellationToken ct = default)
    {
        var query = _db.PurchaseReceipts.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new PurchaseReceiptListResponse
            {
                Id = e.Id,
                SupplierId = e.SupplierId,
                PurchaseOrderId = e.PurchaseOrderId,
                WarehouseId = e.WarehouseId,
                StockDocumentId = e.StockDocumentId,
                ReceiptNo = e.ReceiptNo,
                ReceiptDate = e.ReceiptDate,
                Status = e.Status,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<PurchaseReceiptListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<PurchaseReceiptListResponse>>.Success(page);
    }

    public async Task<BaseResponse<PurchaseReceiptDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.PurchaseReceipts.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new PurchaseReceiptDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                SupplierId = e.SupplierId,
                PurchaseOrderId = e.PurchaseOrderId,
                WarehouseId = e.WarehouseId,
                StockDocumentId = e.StockDocumentId,
                ReceiptNo = e.ReceiptNo,
                ReceiptDate = e.ReceiptDate,
                Status = e.Status
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<PurchaseReceiptDetailResponse>.Failure("NotFound")
            : BaseResponse<PurchaseReceiptDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreatePurchaseReceiptRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Procurement.PurchaseReceipt
        {
            Id = Guid.NewGuid(),
            SupplierId = request.SupplierId,
            PurchaseOrderId = request.PurchaseOrderId,
            WarehouseId = request.WarehouseId,
            StockDocumentId = request.StockDocumentId,
            ReceiptNo = request.ReceiptNo,
            ReceiptDate = request.ReceiptDate,
            Status = request.Status,
            CreatedAt = DateTime.UtcNow,
        };
        _db.PurchaseReceipts.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePurchaseReceiptRequest request, CancellationToken ct = default)
    {
        var entity = await _db.PurchaseReceipts.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.SupplierId = request.SupplierId;
            entity.PurchaseOrderId = request.PurchaseOrderId;
            entity.WarehouseId = request.WarehouseId;
            entity.StockDocumentId = request.StockDocumentId;
            entity.ReceiptNo = request.ReceiptNo;
            entity.ReceiptDate = request.ReceiptDate;
            entity.Status = request.Status;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.PurchaseReceipts.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
