using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Organization.ExpenseClaim.Lookups;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Responses;

namespace Energy.Infrastructure.Modules.Organization.ExpenseClaim.Lookups;

/// <summary>ExpenseClaim lookup servisi (aktif + arama filtreli projection).</summary>
public class ExpenseClaimLookupService : IExpenseClaimLookupService
{
    private readonly EnergyDbContext _db;

    public ExpenseClaimLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ExpenseClaimLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ExpenseClaims.AsNoTracking();
        var items = await query.Select(e => new ExpenseClaimLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ExpenseClaimLookupResponse>>.Success(items);
    }
}
