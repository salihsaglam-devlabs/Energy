using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Lookups;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Responses;

namespace Energy.Infrastructure.Modules.ProgressPayments.ProgressPaymentLine.Lookups;

/// <summary>ProgressPaymentLine lookup servisi (aktif + arama filtreli projection).</summary>
public class ProgressPaymentLineLookupService : IProgressPaymentLineLookupService
{
    private readonly AppDbContext _db;

    public ProgressPaymentLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProgressPaymentLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProgressPaymentLines.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new ProgressPaymentLineLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ProgressPaymentLineLookupResponse>>.Success(items);
    }
}
