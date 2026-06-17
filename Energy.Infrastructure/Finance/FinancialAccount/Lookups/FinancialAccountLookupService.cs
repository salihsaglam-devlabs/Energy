using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Finance.FinancialAccount.Lookups;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Responses;

namespace Energy.Infrastructure.Finance.FinancialAccount.Lookups;

/// <summary>FinancialAccount lookup servisi (aktif + arama filtreli projection).</summary>
public class FinancialAccountLookupService : IFinancialAccountLookupService
{
    private readonly AppDbContext _db;

    public FinancialAccountLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<FinancialAccountLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.FinancialAccounts.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new FinancialAccountLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<FinancialAccountLookupResponse>>.Success(items);
    }
}
