using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.Reports.PayableAging.Services;
using Energy.Shared.Models.V1.Finance.Reports.PayableAging.Requests;
using Energy.Shared.Models.V1.Finance.Reports.PayableAging.Responses;

namespace Energy.Infrastructure.Modules.Finance.Reports.PayableAging;

/// <summary>PayableAging raporu servisi (AsNoTracking, projection, filtre, sayfalama).</summary>
public sealed class PayableAgingService : IPayableAgingService
{
    private readonly AppDbContext _db;

    public PayableAgingService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<PayableAgingRowResponse>>> GetDataAsync(PayableAgingRequest request, CancellationToken ct = default)
    {
        var query = _db.Payables.AsNoTracking();
        if (request.StartDate.HasValue) query = query.Where(e => e.DueDate >= request.StartDate.Value);
        if (request.EndDate.HasValue) query = query.Where(e => e.DueDate <= request.EndDate.Value);
        var total = await query.CountAsync(ct);
        var pageSize = request.PageSize <= 0 ? 50 : request.PageSize;
        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var items = await query
            .OrderByDescending(e => e.DueDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new PayableAgingRowResponse
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
        var page = PaginatedResponse<PayableAgingRowResponse>.Create(items, pageNumber, pageSize, total);
        return BaseResponse<PaginatedResponse<PayableAgingRowResponse>>.Success(page);
    }
}
