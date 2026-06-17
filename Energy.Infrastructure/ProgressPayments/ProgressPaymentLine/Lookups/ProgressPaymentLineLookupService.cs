using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.ProgressPayments.ProgressPaymentLine.Lookups;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Responses;

namespace Energy.Infrastructure.ProgressPayments.ProgressPaymentLine.Lookups;

/// <summary>ProgressPaymentLine lookup servisi (aktif + arama filtreli projection).</summary>
public class ProgressPaymentLineLookupService : IProgressPaymentLineLookupService
{
    private readonly AppDbContext _db;

    public ProgressPaymentLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProgressPaymentLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProgressPaymentLines.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ProgressPaymentLineLookupResponse>)rows.Select(e => new ProgressPaymentLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.Description ?? "") + " - " + e.Quantity.ToString()) ? "Progress Payment Line #" + e.Id.ToString().Substring(0, 8) : ((e.Description ?? "") + " - " + e.Quantity.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ProgressPaymentLineLookupResponse>>.Success(items);
    }
}
