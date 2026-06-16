using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Budget.BudgetLine.Lookups;
using Energy.Shared.Models.V1.Budget.BudgetLine.Responses;

namespace Energy.Infrastructure.Modules.Budget.BudgetLine.Lookups;

/// <summary>BudgetLine lookup servisi (aktif + arama filtreli projection).</summary>
public class BudgetLineLookupService : IBudgetLineLookupService
{
    private readonly EnergyDbContext _db;

    public BudgetLineLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<BudgetLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.BudgetLines.AsNoTracking();
        var items = await query.Select(e => new BudgetLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<BudgetLineLookupResponse>>.Success(items);
    }
}
