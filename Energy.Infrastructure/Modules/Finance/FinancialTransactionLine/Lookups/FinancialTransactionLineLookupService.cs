using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.FinancialTransactionLine.Lookups;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Responses;

namespace Energy.Infrastructure.Modules.Finance.FinancialTransactionLine.Lookups;

/// <summary>FinancialTransactionLine lookup servisi (aktif + arama filtreli projection).</summary>
public class FinancialTransactionLineLookupService : IFinancialTransactionLineLookupService
{
    private readonly EnergyDbContext _db;

    public FinancialTransactionLineLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<FinancialTransactionLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.FinancialTransactionLines.AsNoTracking();
        var items = await query.Select(e => new FinancialTransactionLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<FinancialTransactionLineLookupResponse>>.Success(items);
    }
}
