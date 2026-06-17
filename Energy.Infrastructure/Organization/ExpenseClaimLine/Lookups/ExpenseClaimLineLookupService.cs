using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Organization.ExpenseClaimLine.Lookups;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Responses;

namespace Energy.Infrastructure.Organization.ExpenseClaimLine.Lookups;

/// <summary>ExpenseClaimLine lookup servisi (aktif + arama filtreli projection).</summary>
public class ExpenseClaimLineLookupService : IExpenseClaimLineLookupService
{
    private readonly AppDbContext _db;

    public ExpenseClaimLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ExpenseClaimLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ExpenseClaimLines.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new ExpenseClaimLineLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ExpenseClaimLineLookupResponse>>.Success(items);
    }
}
