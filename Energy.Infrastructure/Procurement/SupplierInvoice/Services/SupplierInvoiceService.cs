using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Procurement.SupplierInvoice.Services;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Responses;

namespace Energy.Infrastructure.Procurement.SupplierInvoice.Services;

/// <summary>SupplierInvoice CRUD servisi (projection, pagination, soft-delete).</summary>
public class SupplierInvoiceService : ISupplierInvoiceService
{
    private readonly AppDbContext _db;

    public SupplierInvoiceService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<SupplierInvoiceListResponse>>> GetListAsync(GetSupplierInvoiceListRequest request, CancellationToken ct = default)
    {
        var query = _db.SupplierInvoices.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new SupplierInvoiceListResponse
            {
                Id = e.Id,
                SupplierId = e.SupplierId,
                PurchaseOrderId = e.PurchaseOrderId,
                PurchaseReceiptId = e.PurchaseReceiptId,
                CurrencyId = e.CurrencyId,
                InvoiceNo = e.InvoiceNo,
                InvoiceDate = e.InvoiceDate,
                TotalAmount = e.TotalAmount,
                Status = e.Status,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<SupplierInvoiceListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<SupplierInvoiceListResponse>>.Success(page);
    }

    public async Task<BaseResponse<SupplierInvoiceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.SupplierInvoices.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new SupplierInvoiceDetailResponse
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
                PurchaseReceiptId = e.PurchaseReceiptId,
                CurrencyId = e.CurrencyId,
                InvoiceNo = e.InvoiceNo,
                InvoiceDate = e.InvoiceDate,
                TotalAmount = e.TotalAmount,
                Status = e.Status
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<SupplierInvoiceDetailResponse>.Failure("NotFound")
            : BaseResponse<SupplierInvoiceDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateSupplierInvoiceRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Procurement.SupplierInvoice
        {
            Id = Guid.NewGuid(),
            SupplierId = request.SupplierId,
            PurchaseOrderId = request.PurchaseOrderId,
            PurchaseReceiptId = request.PurchaseReceiptId,
            CurrencyId = request.CurrencyId,
            InvoiceNo = request.InvoiceNo,
            InvoiceDate = request.InvoiceDate,
            TotalAmount = request.TotalAmount,
            Status = request.Status,
            CreatedAt = DateTime.UtcNow,
        };
        _db.SupplierInvoices.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSupplierInvoiceRequest request, CancellationToken ct = default)
    {
        var entity = await _db.SupplierInvoices.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.SupplierId = request.SupplierId;
            entity.PurchaseOrderId = request.PurchaseOrderId;
            entity.PurchaseReceiptId = request.PurchaseReceiptId;
            entity.CurrencyId = request.CurrencyId;
            entity.InvoiceNo = request.InvoiceNo;
            entity.InvoiceDate = request.InvoiceDate;
            entity.TotalAmount = request.TotalAmount;
            entity.Status = request.Status;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.SupplierInvoices.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
