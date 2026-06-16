using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.ProgressPayments.ProgressPayment.Lookups;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Responses;

namespace Energy.Infrastructure.Modules.ProgressPayments.ProgressPayment.Lookups;

/// <summary>ProgressPayment lookup servisi (aktif + arama filtreli projection).</summary>
public class ProgressPaymentLookupService : IProgressPaymentLookupService
{
    private readonly AppDbContext _db;

    public ProgressPaymentLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProgressPaymentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProgressPayments.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new ProgressPaymentLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ProgressPaymentLookupResponse>>.Success(items);
    }
}
