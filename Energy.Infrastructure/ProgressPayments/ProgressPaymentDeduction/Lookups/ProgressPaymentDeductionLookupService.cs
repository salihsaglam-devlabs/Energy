using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.ProgressPayments.ProgressPaymentDeduction.Lookups;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Responses;

namespace Energy.Infrastructure.ProgressPayments.ProgressPaymentDeduction.Lookups;

/// <summary>ProgressPaymentDeduction lookup servisi (aktif + arama filtreli projection).</summary>
public class ProgressPaymentDeductionLookupService : IProgressPaymentDeductionLookupService
{
    private readonly AppDbContext _db;

    public ProgressPaymentDeductionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProgressPaymentDeductionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProgressPaymentDeductions.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ProgressPaymentDeductionLookupResponse>)rows.Select(e => new ProgressPaymentDeductionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.Note ?? "") + " - " + (e.DeductionType ?? "")) ? "Progress Payment Deduction #" + e.Id.ToString().Substring(0, 8) : ((e.Note ?? "") + " - " + (e.DeductionType ?? "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ProgressPaymentDeductionLookupResponse>>.Success(items);
    }
}
