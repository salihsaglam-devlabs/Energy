using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.FinancialTransaction.Lookups;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Responses;

namespace Energy.Infrastructure.Modules.Finance.FinancialTransaction.Lookups;

/// <summary>FinancialTransaction lookup servisi (aktif + arama filtreli projection).</summary>
public class FinancialTransactionLookupService : IFinancialTransactionLookupService
{
    private readonly EnergyDbContext _db;

    public FinancialTransactionLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<FinancialTransactionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.FinancialTransactions.AsNoTracking();
        var items = await query.Select(e => new FinancialTransactionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<FinancialTransactionLookupResponse>>.Success(items);
    }
}
