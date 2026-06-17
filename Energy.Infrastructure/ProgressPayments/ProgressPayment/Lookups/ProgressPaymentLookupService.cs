using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.ProgressPayments.ProgressPayment.Lookups;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Responses;

namespace Energy.Infrastructure.ProgressPayments.ProgressPayment.Lookups;

/// <summary>ProgressPayment lookup servisi (aktif + arama filtreli projection).</summary>
public class ProgressPaymentLookupService : IProgressPaymentLookupService
{
    private readonly AppDbContext _db;

    public ProgressPaymentLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProgressPaymentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProgressPayments.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.ProgressPaymentNo)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ProgressPaymentLookupResponse>)rows.Select(e => new ProgressPaymentLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.ProgressPaymentNo ?? "") + " - " + e.Status.ToString()) ? "Progress Payment #" + e.Id.ToString().Substring(0, 8) : ((e.ProgressPaymentNo ?? "") + " - " + e.Status.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ProgressPaymentLookupResponse>>.Success(items);
    }
}
