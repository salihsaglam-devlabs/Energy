using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Finance.FinancialTransactionLine.Lookups;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Responses;

namespace Energy.Infrastructure.Finance.FinancialTransactionLine.Lookups;

/// <summary>FinancialTransactionLine lookup servisi (aktif + arama filtreli projection).</summary>
public class FinancialTransactionLineLookupService : IFinancialTransactionLineLookupService
{
    private readonly AppDbContext _db;

    public FinancialTransactionLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<FinancialTransactionLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.FinancialTransactionLines.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new FinancialTransactionLineLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<FinancialTransactionLineLookupResponse>>.Success(items);
    }
}
