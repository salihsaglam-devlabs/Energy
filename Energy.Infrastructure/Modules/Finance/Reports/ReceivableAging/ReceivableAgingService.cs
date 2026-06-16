using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.Reports.ReceivableAging.Services;
using Energy.Shared.Models.V1.Finance.Reports.ReceivableAging.Requests;
using Energy.Shared.Models.V1.Finance.Reports.ReceivableAging.Responses;

namespace Energy.Infrastructure.Modules.Finance.Reports.ReceivableAging;

/// <summary>ReceivableAging raporu servisi (AsNoTracking, projection, filtre, sayfalama).</summary>
public sealed class ReceivableAgingService : IReceivableAgingService
{
    private readonly EnergyDbContext _db;

    public ReceivableAgingService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ReceivableAgingRowResponse>>> GetDataAsync(ReceivableAgingRequest request, CancellationToken ct = default)
    {
        var query = _db.Receivables.AsNoTracking();
        if (request.StartDate.HasValue) query = query.Where(e => e.DueDate >= request.StartDate.Value);
        if (request.EndDate.HasValue) query = query.Where(e => e.DueDate <= request.EndDate.Value);
        var total = await query.CountAsync(ct);
        var pageSize = request.PageSize <= 0 ? 50 : request.PageSize;
        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var items = await query
            .OrderByDescending(e => e.DueDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new ReceivableAgingRowResponse
            {
                Id = e.Id,
                PartnerId = e.PartnerId,
                CurrencyId = e.CurrencyId,
                Amount = e.Amount,
                RemainingAmount = e.RemainingAmount,
                DueDate = e.DueDate,
                IsClosed = e.IsClosed
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ReceivableAgingRowResponse>.Create(items, pageNumber, pageSize, total);
        return BaseResponse<PaginatedResponse<ReceivableAgingRowResponse>>.Success(page);
    }
}
