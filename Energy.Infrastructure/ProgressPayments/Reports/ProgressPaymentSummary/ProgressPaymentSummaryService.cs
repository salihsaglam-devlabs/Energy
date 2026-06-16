using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.ProgressPayments.Reports.ProgressPaymentSummary.Services;
using Energy.Shared.Models.V1.ProgressPayments.Reports.ProgressPaymentSummary.Requests;
using Energy.Shared.Models.V1.ProgressPayments.Reports.ProgressPaymentSummary.Responses;

namespace Energy.Infrastructure.ProgressPayments.Reports.ProgressPaymentSummary;

/// <summary>ProgressPaymentSummary raporu servisi (AsNoTracking, projection, filtre, sayfalama).</summary>
public sealed class ProgressPaymentSummaryService : IProgressPaymentSummaryService
{
    private readonly AppDbContext _db;

    public ProgressPaymentSummaryService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ProgressPaymentSummaryRowResponse>>> GetDataAsync(ProgressPaymentSummaryRequest request, CancellationToken ct = default)
    {
        var query = _db.ProgressPayments.AsNoTracking();
        if (request.StartDate.HasValue) query = query.Where(e => e.PaymentPeriodStart >= request.StartDate.Value);
        if (request.EndDate.HasValue) query = query.Where(e => e.PaymentPeriodStart <= request.EndDate.Value);
        if (!string.IsNullOrWhiteSpace(request.Status)) query = query.Where(e => e.Status.ToString() == request.Status);
        var total = await query.CountAsync(ct);
        var pageSize = request.PageSize <= 0 ? 50 : request.PageSize;
        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var items = await query
            .OrderByDescending(e => e.PaymentPeriodStart)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new ProgressPaymentSummaryRowResponse
            {
                Id = e.Id,
                ProgressPaymentNo = e.ProgressPaymentNo,
                ContractId = e.ContractId,
                GrossAmount = e.GrossAmount,
                NetAmount = e.NetAmount,
                PaymentPeriodStart = e.PaymentPeriodStart,
                Status = e.Status.ToString()
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ProgressPaymentSummaryRowResponse>.Create(items, pageNumber, pageSize, total);
        return BaseResponse<PaginatedResponse<ProgressPaymentSummaryRowResponse>>.Success(page);
    }
}
