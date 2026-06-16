using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Budget.Budget.Lookups;
using Energy.Shared.Models.V1.Budget.Budget.Responses;

namespace Energy.Infrastructure.Modules.Budget.Budget.Lookups;

/// <summary>Budget lookup servisi (aktif + arama filtreli projection).</summary>
public class BudgetLookupService : IBudgetLookupService
{
    private readonly EnergyDbContext _db;

    public BudgetLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<BudgetLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Budgets.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query.Select(e => new BudgetLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = e.Name,
            DisplayName = e.Name,
            IsActive = e.IsActive
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<BudgetLookupResponse>>.Success(items);
    }
}
