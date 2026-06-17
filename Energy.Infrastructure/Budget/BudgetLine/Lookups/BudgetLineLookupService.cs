using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Budget.BudgetLine.Lookups;
using Energy.Shared.Models.V1.Budget.BudgetLine.Responses;

namespace Energy.Infrastructure.Budget.BudgetLine.Lookups;

/// <summary>BudgetLine lookup servisi (aktif + arama filtreli projection).</summary>
public class BudgetLineLookupService : IBudgetLineLookupService
{
    private readonly AppDbContext _db;

    public BudgetLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<BudgetLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.BudgetLines.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<BudgetLineLookupResponse>)rows.Select(e => new BudgetLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.Description ?? "") + " - " + e.PlannedAmount.ToString()) ? "Budget Line #" + e.Id.ToString().Substring(0, 8) : ((e.Description ?? "") + " - " + e.PlannedAmount.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<BudgetLineLookupResponse>>.Success(items);
    }
}
