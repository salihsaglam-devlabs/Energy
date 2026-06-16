using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Organization.ExpenseClaimLine.Lookups;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Responses;

namespace Energy.Infrastructure.Modules.Organization.ExpenseClaimLine.Lookups;

/// <summary>ExpenseClaimLine lookup servisi (aktif + arama filtreli projection).</summary>
public class ExpenseClaimLineLookupService : IExpenseClaimLineLookupService
{
    private readonly EnergyDbContext _db;

    public ExpenseClaimLineLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ExpenseClaimLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ExpenseClaimLines.AsNoTracking();
        var items = await query.Select(e => new ExpenseClaimLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ExpenseClaimLineLookupResponse>>.Success(items);
    }
}
