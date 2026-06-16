using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Inventory.Reports.StockBalanceReport.Services;
using Energy.Shared.Models.V1.Inventory.Reports.StockBalanceReport.Requests;
using Energy.Shared.Models.V1.Inventory.Reports.StockBalanceReport.Responses;

namespace Energy.Infrastructure.Inventory.Reports.StockBalanceReport;

/// <summary>StockBalanceReport raporu servisi (AsNoTracking, projection, filtre, sayfalama).</summary>
public sealed class StockBalanceReportService : IStockBalanceReportService
{
    private readonly AppDbContext _db;

    public StockBalanceReportService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<StockBalanceReportRowResponse>>> GetDataAsync(StockBalanceReportRequest request, CancellationToken ct = default)
    {
        var query = _db.StockBalances.AsNoTracking();
        if (request.StartDate.HasValue) query = query.Where(e => e.LastRecalculatedAt >= request.StartDate.Value);
        if (request.EndDate.HasValue) query = query.Where(e => e.LastRecalculatedAt <= request.EndDate.Value);
        var total = await query.CountAsync(ct);
        var pageSize = request.PageSize <= 0 ? 50 : request.PageSize;
        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var items = await query
            .OrderByDescending(e => e.LastRecalculatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new StockBalanceReportRowResponse
            {
                Id = e.Id,
                WarehouseId = e.WarehouseId,
                MaterialId = e.MaterialId,
                Quantity = e.Quantity,
                ReservedQuantity = e.ReservedQuantity,
                TotalCost = e.TotalCost,
                LastRecalculatedAt = e.LastRecalculatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<StockBalanceReportRowResponse>.Create(items, pageNumber, pageSize, total);
        return BaseResponse<PaginatedResponse<StockBalanceReportRowResponse>>.Success(page);
    }
}
