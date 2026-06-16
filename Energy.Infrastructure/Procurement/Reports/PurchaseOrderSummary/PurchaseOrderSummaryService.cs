using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Procurement.Reports.PurchaseOrderSummary.Services;
using Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Requests;
using Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Responses;

namespace Energy.Infrastructure.Procurement.Reports.PurchaseOrderSummary;

/// <summary>PurchaseOrderSummary raporu servisi (AsNoTracking, projection, filtre, sayfalama).</summary>
public sealed class PurchaseOrderSummaryService : IPurchaseOrderSummaryService
{
    private readonly AppDbContext _db;

    public PurchaseOrderSummaryService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<PurchaseOrderSummaryRowResponse>>> GetDataAsync(PurchaseOrderSummaryRequest request, CancellationToken ct = default)
    {
        var query = _db.PurchaseOrders.AsNoTracking();
        if (request.StartDate.HasValue) query = query.Where(e => e.OrderDate >= request.StartDate.Value);
        if (request.EndDate.HasValue) query = query.Where(e => e.OrderDate <= request.EndDate.Value);
        if (!string.IsNullOrWhiteSpace(request.Status)) query = query.Where(e => e.Status.ToString() == request.Status);
        var total = await query.CountAsync(ct);
        var pageSize = request.PageSize <= 0 ? 50 : request.PageSize;
        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var items = await query
            .OrderByDescending(e => e.OrderDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new PurchaseOrderSummaryRowResponse
            {
                Id = e.Id,
                OrderNo = e.OrderNo,
                OrderDate = e.OrderDate,
                SupplierId = e.SupplierId,
                ProjectId = e.ProjectId,
                CurrencyId = e.CurrencyId,
                Status = e.Status.ToString()
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<PurchaseOrderSummaryRowResponse>.Create(items, pageNumber, pageSize, total);
        return BaseResponse<PaginatedResponse<PurchaseOrderSummaryRowResponse>>.Success(page);
    }
}
