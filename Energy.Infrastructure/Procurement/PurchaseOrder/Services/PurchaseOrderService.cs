using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Procurement.PurchaseOrder.Services;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Responses;

namespace Energy.Infrastructure.Procurement.PurchaseOrder.Services;

/// <summary>PurchaseOrder CRUD servisi (projection, pagination, soft-delete).</summary>
public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly AppDbContext _db;

    public PurchaseOrderService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<PurchaseOrderListResponse>>> GetListAsync(GetPurchaseOrderListRequest request, CancellationToken ct = default)
    {
        var query = _db.PurchaseOrders.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new PurchaseOrderListResponse
            {
                Id = e.Id,
                SupplierId = e.SupplierId,
                ProjectId = e.ProjectId,
                Status = e.Status,
                OrderNo = e.OrderNo,
                CurrencyId = e.CurrencyId,
                OrderDate = e.OrderDate,
                ApprovalRequestId = e.ApprovalRequestId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<PurchaseOrderListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<PurchaseOrderListResponse>>.Success(page);
    }

    public async Task<BaseResponse<PurchaseOrderDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.PurchaseOrders.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new PurchaseOrderDetailResponse
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
                ProjectId = e.ProjectId,
                Status = e.Status,
                OrderNo = e.OrderNo,
                CurrencyId = e.CurrencyId,
                OrderDate = e.OrderDate,
                ApprovalRequestId = e.ApprovalRequestId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<PurchaseOrderDetailResponse>.Failure("NotFound")
            : BaseResponse<PurchaseOrderDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreatePurchaseOrderRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Procurement.PurchaseOrder
        {
            Id = Guid.NewGuid(),
            SupplierId = request.SupplierId,
            ProjectId = request.ProjectId,
            Status = request.Status,
            OrderNo = request.OrderNo,
            CurrencyId = request.CurrencyId,
            OrderDate = request.OrderDate,
            ApprovalRequestId = request.ApprovalRequestId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.PurchaseOrders.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePurchaseOrderRequest request, CancellationToken ct = default)
    {
        var entity = await _db.PurchaseOrders.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.SupplierId = request.SupplierId;
            entity.ProjectId = request.ProjectId;
            entity.Status = request.Status;
            entity.OrderNo = request.OrderNo;
            entity.CurrencyId = request.CurrencyId;
            entity.OrderDate = request.OrderDate;
            entity.ApprovalRequestId = request.ApprovalRequestId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.PurchaseOrders.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
