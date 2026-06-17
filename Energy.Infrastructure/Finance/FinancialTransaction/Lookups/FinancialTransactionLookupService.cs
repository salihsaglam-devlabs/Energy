using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Finance.FinancialTransaction.Lookups;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Responses;

namespace Energy.Infrastructure.Finance.FinancialTransaction.Lookups;

/// <summary>FinancialTransaction lookup servisi (aktif + arama filtreli projection).</summary>
public class FinancialTransactionLookupService : IFinancialTransactionLookupService
{
    private readonly AppDbContext _db;

    public FinancialTransactionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<FinancialTransactionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.FinancialTransactions.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new FinancialTransactionLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<FinancialTransactionLookupResponse>>.Success(items);
    }
}
